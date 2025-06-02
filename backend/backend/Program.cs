using backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;


// Сваггер
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TodoList API",
        Version = "v1",
        Description = "API for registration"
    });

	var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Введите токен JWT, полученный после логина. Пример: Bearer {токен}",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"] 
                 ?? throw new InvalidOperationException("JWT Key not configured");
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();



// Авторизация 
app.MapPost("/login", async (LoginRequest request, AppDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
    {
        return Results.Unauthorized();
    }

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

    
    var jwtKey = builder.Configuration["Jwt:Key"] 
                 ?? throw new InvalidOperationException("JWT Key not configured");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Issuer"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: creds 
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new { Token = tokenString, Role = user.Role, UserId = user.UserId });
})
.WithOpenApi(operation => new(operation)
{
    Summary = "User login",
    Description = "Authenticates a user and returns a JWT token",
    Tags = new List<OpenApiTag> { new() { Name = "Authentication" } }
});

// БД
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API v1");
    });
}



// Эндпоинт регистра
app.MapPost("/register", async (RegisterRequest request, AppDbContext db) =>
{
    // Validate email uniqueness
    if (await db.Users.AnyAsync(u => u.Email == request.Email))
    {
        return Results.Conflict("Email already exists");
    }

    var user = new User
    {
        Name = request.Name,
        Email = request.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        Role = request.IsParent ? "parent" : "child",
        CreatedAt = DateTime.UtcNow.Date 
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok(new { 
        Message = "Registration successful",
        UserId = user.UserId,
        Role = user.Role
    });
})
.WithOpenApi(operation => new(operation)
{
    Summary = "Register a new user",
    Description = "Creates a new user account with either parent or child role",
    Tags = new List<OpenApiTag> { new() { Name = "Authentication" } }
});

// Создание задания
app.MapPost("/tasks", async (TaskCreateRequest request, AppDbContext db) =>
{
    var task = new TaskItem
    {
        ParentId = request.ParentId,
        ChildId = request.ChildId,
        Description = request.Description,
        Deadline = request.Deadline,
        Reward = request.Reward,
        Status = request.Status ?? "ongoing"
    };

    await db.Tasks.AddAsync(task);
    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
}).RequireAuthorization(policy => policy.RequireRole("parent"));


// Получение всех задач родителя
app.MapGet("/tasks/parent/{parentId}", async (int parentId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ParentId == parentId)
        .ToListAsync();
    return Results.Ok(tasks);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Редактирование задания
app.MapPut("/tasks/{id}", async (int id, TaskItem updatedTask, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    task.Description = updatedTask.Description;
    task.Deadline = updatedTask.Deadline;
    task.Reward = updatedTask.Reward;
    task.Status = updatedTask.Status;

    await db.SaveChangesAsync();
    return Results.Ok(task);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Удаление задания
app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.Ok();
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Подтверждение выполнения задания 
app.MapPost("/tasks/{id}/approve", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null || task.Status != "pending")
        return Results.BadRequest("Задача не найдена или статус некорректен.");

    task.Status = "completed";
    await db.SaveChangesAsync();
    return Results.Ok(task);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Отклонение выполнения задания 
app.MapPost("/tasks/{taskId}/reject", async (int taskId, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(taskId);

    if (task is null || task.Status != "pending")
        return Results.BadRequest("Задача не найдена или статус некорректен.");

    task.Status = "ongoing";
    await db.SaveChangesAsync();

    return Results.Ok(task);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Показать задания, которые ребенок отметил выполненными (на проверке)
app.MapGet("/tasks/parent/{parentId}/pending", async (int parentId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ParentId == parentId && t.Status == "pending")
        .ToListAsync();
    return Results.Ok(tasks);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

// Показать задания, которые ребенок еще не выполнил
app.MapGet("/tasks/parent/{parentId}/notcompleted", async (int parentId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ParentId == parentId && t.Status == "ongoing")
        .ToListAsync();
    return Results.Ok(tasks);
}).RequireAuthorization(policy => policy.RequireRole("parent"));

//РЕБЕНОК
// Получение всех задач ребенка
app.MapGet("/tasks/child/{childId}", async (int childId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ChildId == childId)
        .ToListAsync();
    return Results.Ok(tasks);
}).RequireAuthorization(policy => policy.RequireRole("child"));

// Получение не выполненных задач ребенка
app.MapGet("/tasks/child/{childId}/active", async (int childId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ChildId == childId && t.Status == "ongoing")
        .ToListAsync();
    return Results.Ok(tasks);
}).RequireAuthorization(policy => policy.RequireRole("child"));

// Пометить задачу как "выполненную" у ребенка
app.MapPost("/tasks/{id}/complete", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null || task.Status != "ongoing")
        return Results.BadRequest("Задача не найдена или уже в статусе pending/completed.");
    task.Status = "pending";
    await db.SaveChangesAsync();
    return Results.Ok(task);
}).RequireAuthorization(policy => policy.RequireRole("child"));




app.Run();

public record LoginRequest(string Email, string Password);

public record RegisterRequest(string Name, string Email, string Password, bool IsParent);

public record TaskCreateRequest(
    int ParentId,
    int ChildId,
    string Description,
    DateTime? Deadline,
    string? Reward,
    string? Status);
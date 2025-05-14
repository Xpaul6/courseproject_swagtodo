using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

<<<<<<< HEAD

// БД
=======
>>>>>>> a1a0559f7e46087aad19dafeba9d6c40810fadb2
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

<<<<<<< HEAD


=======
>>>>>>> a1a0559f7e46087aad19dafeba9d6c40810fadb2
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
});


// Получение всех задач родителя
app.MapGet("/tasks/parent/{parentId}", async (int parentId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ParentId == parentId)
        .ToListAsync();
    return Results.Ok(tasks);
});

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
});

// Удаление задания
app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Подтверждение выполнения задания 
app.MapPost("/tasks/{id}/approve", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null || task.Status != "pending")
        return Results.BadRequest("Задача не найдена или статус некорректен.");

    task.Status = "completed"; 
    await db.SaveChangesAsync();
    return Results.Ok(task);
});



app.Run();

public record RegisterRequest(string Name, string Email, string Password, bool IsParent);

public record TaskCreateRequest(
    int ParentId,
    int ChildId,
    string Description,
    DateTime? Deadline,
    string? Reward,
    string? Status);
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

// �������
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API v1");
    });
}


// �������� ��������
app.MapPost("/register", async (RegisterRequest request, AppDbContext db) =>
{
    var user = new User
    {
        Name = request.Name,
        Email = request.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        Role = request.IsParent ? "parent" : "child"
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "�������� �����������" });
})
.WithOpenApi(operation => new(operation)
{
    Summary = "Registration",
    Description = "Register new person with role",
    Tags = new List<OpenApiTag>
    {
        new OpenApiTag { Name = "Authentication" }
    }
});

// �������� �������
app.MapPost("/tasks", async (TaskItem task, AppDbContext db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

// ��������� ���� ����� ��������
app.MapGet("/tasks/parent/{parentId}", async (int parentId, AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(t => t.ParentId == parentId)
        .ToListAsync();
    return Results.Ok(tasks);
});

// �������������� �������
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

// �������� �������
app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// ������������� ���������� ������� 
app.MapPost("/tasks/{id}/approve", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null || task.Status != "pending")
        return Results.BadRequest("������ �� ������� ��� ������ �����������.");

    task.Status = "completed"; 
    await db.SaveChangesAsync();
    return Results.Ok(task);
});


app.Run();

public record RegisterRequest(string Name, string Email, string Password, bool IsParent);

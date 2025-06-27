using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;
namespace backend.Controllers;

// Контроллер задачами
[ApiController]
public class TaskController : ControllerBase
{
    // Контекст базы данных
    private readonly AppDbContext _db;

    // Конструктор контроллера
    public TaskController(AppDbContext db)
    {
        _db = db;
    }

    // Создания новой задачи
    [HttpPost("tasks")] 
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> CreateTask(TaskCreateRequest request)
    {
        var taskList = await _db.TaskLists
            .FirstOrDefaultAsync(tl => tl.ListId == request.TaskListId && tl.ParentId == request.ParentId);
        
        if (taskList is null)
        {
            return Results.BadRequest("Такой список не сущесвтуент или не принадлежит данному родителю");
        }

        if (taskList.ChildId != request.ChildId)
        {
            return Results.BadRequest("Список не пренадлежит этому ребенку");
        }

        var task = new TaskItem
        {
            ParentId = request.ParentId,
            ChildId = request.ChildId,
            TaskListId = request.TaskListId,
            Description = request.Description,
            Deadline = request.Deadline,
            Reward = request.Reward,
            Status = (request.Status == "pending" || request.Status == "completed") 
                ? request.Status 
                : "ongoing",
            CreatedAt = DateTime.UtcNow.Date
        };

        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();

        return Results.Created($"/tasks/{task.TaskId}", task);
    }

    // Получения всех задач конкретного родителя
    [HttpGet("tasks/parent/{parentId}")] 
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> GetParentTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId)
            .ToListAsync();
        return Results.Ok(tasks);
    }

    // Редактирование существующей задачи
    [HttpPut("tasks/{id}")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> UpdateTask(int id, TaskItem updatedTask)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return Results.NotFound();

        task.Description = updatedTask.Description;
        task.Deadline = updatedTask.Deadline;
        task.Reward = updatedTask.Reward;
        task.Status = updatedTask.Status;

        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }

    // Удаления задачи
    [HttpDelete("tasks/{id}")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> DeleteTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return Results.NotFound();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return Results.Ok();
    }

    // Подтверждения выполнения задачи родителем
    [HttpPost("tasks/{id}/approve")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> ApproveTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "pending")
            return Results.BadRequest("Задача не найдена или статус некорректен");

        task.Status = "completed";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }

    // Отклонения выполнения задачи родителем
    [HttpPost("tasks/{id}/reject")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> RejectTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "pending")
            return Results.BadRequest("Задача не найдена или статус некорректен");

        task.Status = "ongoing";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }

    // Пполучения списка задач для подтверждения родителем
    [HttpGet("tasks/parent/{parentId}/pending")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> GetPendingTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId && t.Status == "pending")
            .ToListAsync();
        return Results.Ok(tasks);
    }

    // Получения списка незавершенных задач
    [HttpGet("tasks/parent/{parentId}/notcompleted")] 
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> GetNotcompletedTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId && t.Status == "ongoing")
            .ToListAsync();
        return Results.Ok(tasks);
    }


    // Получения всех задач конкретного ребенка
    [HttpGet("tasks/child/{childId}")]
    [Authorize(Roles = "child")] // Только для детей
    public async Task<IResult> GetChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId)
            .ToListAsync();
        return Results.Ok(tasks);
    }

    // Получения активных задач ребенка
    [HttpGet("tasks/child/{childId}/active")]
    [Authorize(Roles = "child")] // Только для детей
    public async Task<IResult> GetActiveChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId && t.Status == "ongoing")
            .ToListAsync();
        return Results.Ok(tasks);
    }

    // Получения количества выполненных задач ребенка
    [HttpGet("tasks/child/{childId}/numberofCompleted")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<ActionResult<int>> GetNumberOfCompletedChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId && t.Status == "completed")
            .ToListAsync();
        return Ok(tasks.Count);
    }

    // Отметка задачи как выполненной ребенком
    [HttpPost("tasks/{id}/complete")]
    [Authorize(Roles = "child")] // Только для детей
    public async Task<IResult> CompleteTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "ongoing")
            return Results.BadRequest("Задача не найдена или уже в статусе pending/completed");

        task.Status = "pending";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }
}
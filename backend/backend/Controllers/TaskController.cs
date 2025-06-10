using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;
namespace backend.Controllers;

[ApiController]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _db;

    public TaskController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("tasks")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> CreateTask(TaskCreateRequest request)
    {
        var taskList = await _db.TaskLists
            .FirstOrDefaultAsync(tl => tl.ListId == request.TaskListId && tl.ParentId == request.ParentId);
        
        if (taskList is null)
        {
            return Results.BadRequest("Task list does not exist or does not belong to the parent");
        }

        if (taskList.ChildId != request.ChildId)
        {
            return Results.BadRequest("Child does not match the task list");
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

    [HttpGet("tasks/parent/{parentId}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetParentTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId)
            .ToListAsync();
        return Results.Ok(tasks);
    }

    [HttpPut("tasks/{id}")]
    [Authorize(Roles = "parent")]
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

    [HttpDelete("tasks/{id}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> DeleteTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return Results.NotFound();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return Results.Ok();
    }

    [HttpPost("tasks/{id}/approve")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> ApproveTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "pending")
            return Results.BadRequest("Задача не найдена или статус некорректен.");

        task.Status = "completed";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }

    [HttpPost("tasks/{id}/reject")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> RejectTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "pending")
            return Results.BadRequest("Задача не найдена или статус некорректен.");

        task.Status = "ongoing";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }

    [HttpGet("tasks/parent/{parentId}/pending")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetPendingTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId && t.Status == "pending")
            .ToListAsync();
        return Results.Ok(tasks);
    }

    [HttpGet("tasks/parent/{parentId}/notcompleted")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetNotcompletedTasks(int parentId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ParentId == parentId && t.Status == "ongoing")
            .ToListAsync();
        return Results.Ok(tasks);
    }


    [HttpGet("tasks/child/{childId}")]
    [Authorize(Roles = "child")]
    public async Task<IResult> GetChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId)
            .ToListAsync();
        return Results.Ok(tasks);
    }

    [HttpGet("tasks/child/{childId}/active")]
    [Authorize(Roles = "child")]
    public async Task<IResult> GetActiveChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId && t.Status == "ongoing")
            .ToListAsync();
        return Results.Ok(tasks);
    }

    [HttpGet("tasks/child/{childId}/numberofCompleted")]
    [Authorize(Roles = "parent")]
    public async Task<ActionResult<int>> GetNumberOfCompletedChildTasks(int childId)
    {
        var tasks = await _db.Tasks
            .Where(t => t.ChildId == childId && t.Status == "completed")
            .ToListAsync();
        return Ok(tasks.Count);
    }

    [HttpPost("tasks/{id}/complete")]
    [Authorize(Roles = "child")]
    public async Task<IResult> CompleteTask(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null || task.Status != "ongoing")
            return Results.BadRequest("Задача не найдена или уже в статусе pending/completed.");

        task.Status = "pending";
        await _db.SaveChangesAsync();
        return Results.Ok(task);
    }


}
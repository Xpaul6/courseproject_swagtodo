using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;
namespace backend.Controllers;

[ApiController]
public class TaskListController : ControllerBase
{
    private readonly AppDbContext _db;

    public TaskListController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("tasklists")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> CreateTaskList(TaskListCreateRequest request)
    {
        var parentExists = await _db.Users.AnyAsync(u => u.UserId == request.ParentId && u.Role == "parent");
        var childExists = await _db.Users.AnyAsync(u => u.UserId == request.ChildId && u.Role == "child");
        
        if (!parentExists || !childExists)
        {
            return Results.BadRequest("Parent or child does not exist or has incorrect role");
        }

        var taskList = new TaskList
        {
            ParentId = request.ParentId,
            ChildId = request.ChildId,
            Title = request.Title,
            CreatedAt = DateTime.UtcNow.Date
        };

        _db.TaskLists.Add(taskList);
        await _db.SaveChangesAsync();

        return Results.Created($"/tasklists/{taskList.ListId}", taskList);
    }

    [HttpGet("tasklists/parent/{parentId}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetParentTaskLists(int parentId)
    {
        var taskLists = await _db.TaskLists
            .Where(tl => tl.ParentId == parentId)
            .ToListAsync();
        return Results.Ok(taskLists);
    }

    [HttpGet("tasklists/child/{childId}")]
    [Authorize(Roles = "child")]
    public async Task<IResult> GetChildTaskLists(int childId)
    {
        var taskLists = await _db.TaskLists
            .Where(tl => tl.ChildId == childId)
            .ToListAsync();
        return Results.Ok(taskLists);
    }

    [HttpPut("tasklists/{id}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> UpdateTaskList(int id, TaskListUpdateRequest request)
    {
        var taskList = await _db.TaskLists.FindAsync(id);
        if (taskList is null)
        {
            return Results.NotFound();
        }

        var parentExists = await _db.Users.AnyAsync(u => u.UserId == request.ParentId && u.Role == "parent");
        var childExists = await _db.Users.AnyAsync(u => u.UserId == request.ChildId && u.Role == "child");
        
        if (!parentExists || !childExists)
        {
            return Results.BadRequest("Parent or child does not exist or has incorrect role");
        }

        taskList.ParentId = request.ParentId;
        taskList.ChildId = request.ChildId;
        taskList.Title = request.Title;

        await _db.SaveChangesAsync();
        return Results.Ok(taskList);
    }

    [HttpDelete("tasklists/{id}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> DeleteTaskList(int id)
    {
        var taskList = await _db.TaskLists.FindAsync(id);
        if (taskList is null)
        {
            return Results.NotFound();
        }

        _db.TaskLists.Remove(taskList);
        await _db.SaveChangesAsync();
        return Results.Ok();
    }
}
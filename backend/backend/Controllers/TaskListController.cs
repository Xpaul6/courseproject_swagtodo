using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;
namespace backend.Controllers;


// Контроллер для списков задач 
[ApiController]
public class TaskListController : ControllerBase
{
    // Контекст базы данных
    private readonly AppDbContext _db;


    // Конструктор контроллера
    public TaskListController(AppDbContext db)
    {
        _db = db;
    }


    // Создание нового списка задач для определенного ребенка

    [HttpPost("tasklists")]
    [Authorize(Roles = "parent")] // Только для родителя
    public async Task<IResult> CreateTaskList(TaskListCreateRequest request)
    {
        var parentExists = await _db.Users.AnyAsync(u => u.UserId == request.ParentId && u.Role == "parent");
        var childExists = await _db.Users.AnyAsync(u => u.UserId == request.ChildId && u.Role == "child");
        
        if (!parentExists || !childExists)
        {
            return Results.BadRequest("Родитель или ребенок не существует, или некорректная роль");
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

    // Получение всех списков задач родителя
    [HttpGet("tasklists/parent/{parentId}")]
    [Authorize(Roles = "parent")] // Только для родителя
    public async Task<IResult> GetParentTaskLists(int parentId)
    {
        var taskLists = await _db.TaskLists
            .Where(tl => tl.ParentId == parentId)
            .ToListAsync();
        return Results.Ok(taskLists);
    }

    // Получение всех списков задач ребенка
    [HttpGet("tasklists/child/{childId}")]
    [Authorize(Roles = "child")] // Только для ребенка
    public async Task<IResult> GetChildTaskLists(int childId)
    {
        var taskLists = await _db.TaskLists
            .Where(tl => tl.ChildId == childId)
            .ToListAsync();
        return Results.Ok(taskLists);
    }

    // Редактирование существующего списка задач
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
            return Results.BadRequest("Родитель или ребенок не существует, или некорректная роль");
        }

        taskList.ParentId = request.ParentId;
        taskList.ChildId = request.ChildId;
        taskList.Title = request.Title;

        await _db.SaveChangesAsync();
        return Results.Ok(taskList);
    }

    // Удаление существующего списка задач
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

// Контроллер для связей родителей и детей
[ApiController]
[Route("family")]
public class FamilyController : ControllerBase
{
    // Контекст базы данных
    private readonly AppDbContext _db;

    // Конструктор контроллера
    public FamilyController(AppDbContext db)
    {
        _db = db;
    }

    // Получение существующего или создания нового семейного кода
    [HttpGet("code/{parentId}")] 
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> GetOrCreateFamilyCode(int parentId)
    {
        // Поиск родителя в базе данных по ID и проверка его роли
        var parent = await _db.Users
            .FirstOrDefaultAsync(u => u.UserId == parentId && u.Role == "parent");

        // Родитель не найден, возвращаем ошибку
        if (parent is null)
            return Results.NotFound("Такой родитель не найден");

        // Есть ли уже существующий код для этого родителя
        var existingCode = await _db.FamilyCodes
            .FirstOrDefaultAsync(fc => fc.ParentId == parentId);

        // Существует - возвращаем
        if (existingCode is not null)
            return Results.Ok(new { Code = existingCode.Code });

        // Создаем новый семейный код
        var newCode = new FamilyCode
        {
            ParentId = parentId, // Привязываем код к родителю
            Code = FamilyCode.GenerateUniqueCode() // Генерируем уникальный код
        };

        await _db.FamilyCodes.AddAsync(newCode);
        await _db.SaveChangesAsync();

        return Results.Ok(new { newCode.Code });
    }

    // Присоединения ребенка к семье по коду
    [HttpPost("join")]
    [Authorize(Roles = "child")] // Только для детей
    public async Task<IResult> JoinFamily([FromBody] JoinRequest request)
    {
        // Поиск семейного кода в базе данных
        var codeRecord = await _db.FamilyCodes
            .FirstOrDefaultAsync(c => c.Code == request.Code);

        // Код не найден - возвращаем ошибку
        if (codeRecord is null)
            return Results.BadRequest("Код неверный");

        // Поиск ребенка в базе данных и проверка его роли
        var child = await _db.Users.FindAsync(request.ChildId);

        // Проверяем существование пользователя и его роль
        if (child is null || child.Role != "child")
            return Results.BadRequest("Код неверный");

        child.ParentId = codeRecord.ParentId;
        await _db.SaveChangesAsync();

        return Results.Ok("Привязка успешна");
    }

    // Получения списка всех детей родителя
    [HttpGet("children/{parentId}")]
    [Authorize(Roles = "parent")] // Только для родителей
    public async Task<IResult> GetChildren(int parentId)
    {
        var children = await _db.Users
            .Where(u => u.Role == "child" && u.ParentId == parentId)
            .ToListAsync();

        return Results.Ok(children);
    }
}

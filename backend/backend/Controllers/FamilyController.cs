using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("family")]
public class FamilyController : ControllerBase
{
    private readonly AppDbContext _db;

    public FamilyController(AppDbContext db)
    {
        _db = db;
    }

    // Получение или создание семейного кода
    [HttpGet("code/{parentId}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetOrCreateFamilyCode(int parentId)
    {
        var parent = await _db.Users
            .FirstOrDefaultAsync(u => u.UserId == parentId && u.Role == "parent");

        if (parent is null)
            return Results.NotFound("Parent not found");

        var existingCode = await _db.FamilyCodes
            .FirstOrDefaultAsync(fc => fc.ParentId == parentId);

        if (existingCode is not null)
            return Results.Ok(new { Code = existingCode.Code });

        var newCode = new FamilyCode
        {
            ParentId = parentId,
            Code = FamilyCode.GenerateUniqueCode()
        };

        await _db.FamilyCodes.AddAsync(newCode);
        await _db.SaveChangesAsync();

        return Results.Ok(new { newCode.Code });
    }

    // Присоединение ребенка к родителю по коду
    [HttpPost("join")]
    [Authorize(Roles = "child")]
    public async Task<IResult> JoinFamily([FromBody] JoinRequest request)
    {
        var codeRecord = await _db.FamilyCodes
            .FirstOrDefaultAsync(c => c.Code == request.Code);

        if (codeRecord is null)
            return Results.BadRequest("Код неверный");

        var child = await _db.Users.FindAsync(request.ChildId);

        if (child is null || child.Role != "child")
            return Results.BadRequest("Код неверный");

        child.ParentId = codeRecord.ParentId;
        await _db.SaveChangesAsync();

        return Results.Ok("Привязка успешна");
    }

    // Получение всех детей, привязанных к родителю
    [HttpGet("children/{parentId}")]
    [Authorize(Roles = "parent")]
    public async Task<IResult> GetChildren(int parentId)
    {
        var children = await _db.Users
            .Where(u => u.Role == "child" && u.ParentId == parentId)
            .ToListAsync();

        return Results.Ok(children);
    }
}

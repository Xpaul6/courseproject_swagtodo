using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using backend.DTOs;

namespace backend.Controllers;

// Контроллер аутентификации  (регистрацию и вход пользователей)
[ApiController]
[Route("/")]
[Tags("Authentication")] 
public class AuthController : ControllerBase
{
    // Контекст базы данных для работы с пользователями
    private readonly AppDbContext _db;
    // Конфигурация приложения, где хранятся настройки JWT
    private readonly IConfiguration _configuration;

    // Конструктор
    public AuthController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    // Аутентификацкия пользователя
    [HttpPost("login")]
    public async Task<IResult> Login(LoginRequest request)
    {
        // Поиск пользователя по email в базе данных
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // Проверка существования пользователя и верификация пароля
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Results.Unauthorized();
        }

        // Создание claims для JWT токена
        // Включаем идентификатор пользователя, email и роль
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Получение ключа для JWT из конфигурации
        var jwtKey = _configuration["Jwt:Key"] 
                     ?? throw new InvalidOperationException("JWT Key не сформирован");
        // Создание ключа безопасности из строки конфигурации
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        // Создание учетных данных для подписи токена
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Создание JWT токена с настройкой параметров
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], // Кто выдал токен
            audience: _configuration["Jwt:Issuer"], // Для кого токен
            claims: claims, // Инфа о пользователе
            expires: DateTime.UtcNow.AddDays(1), // Срок действия токена (1 день)
            signingCredentials: creds // Учетные данные для подписи
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token); // Преобразование токена в строку для отправки клиенту

        return Results.Ok(new { Token = tokenString, Role = user.Role, UserId = user.UserId });
    }

    // Регистрации нового пользователя
    [HttpPost("register")]
    public async Task<IResult> Register(RegisterRequest request)
    {
        // Проверка существования пользователя с таким email
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
        {
            return Results.Conflict("Такой email уже существует");
        }

        // Создание нового пользователя
        var user = new User
        {
            Name = request.Name, // Имя пользователя
            Email = request.Email, // Email пользователя
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password), // Хеширование пароля
            Role = request.IsParent ? "parent" : "child", // Определение роли
            CreatedAt = DateTime.UtcNow.Date // Дата создания аккаунта
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Results.Ok(new { 
            Message = "Регистрация успешна",
            UserId = user.UserId,
            Role = user.Role
        });
    }
}
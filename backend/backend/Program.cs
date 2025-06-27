using backend.Configuration;  // Для пользовательских конфигураций приложения
using Microsoft.AspNetCore.Authentication.JwtBearer;  // Для JWT
using Microsoft.EntityFrameworkCore;    // Для работы с Entity Framework Core
using Microsoft.IdentityModel.Tokens;   // Для работы с токенами безопасности
using System.Text;   // Для работы с кодировкой текста

// Создание построителя веб-приложения
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();  // Добавление сервиса для эндпоинтов
builder.Services.ConfigureSwagger();         // Настройка Swagger для документации

// Конфигурация базы данных PostgreSQL
// Регистрация контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации и авторизации
// Конфигурация JWT аутентификации
builder.Services.AddAuthentication(options =>
{
    // Установка схемы аутентификации по умолчанию
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Установка схемы вызова по умолчанию
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Получение ключа JWT из конфигурации
    var jwtKey = builder.Configuration["Jwt:Key"] 
                 ?? throw new InvalidOperationException("JWT Key not configured");
    
    // Настройка параметров токена
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,      
        ValidateAudience = false,   
        ValidateLifetime = true,   
        ValidateIssuerSigningKey = true,  // Включение проверки ключа подписи
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  // Установка ключа подписи
    };
});

// Добавление сервиса авторизации для работы с ролями и разграничения доступа
builder.Services.AddAuthorization();
// Добавление поддержки контроллеров для обработки HTTP-запросов
builder.Services.AddControllers();

// Создание экземпляра приложения на основе настроенного билдера
var app = builder.Build();


// Проверка, находится ли приложение в режиме разработки
if (app.Environment.IsDevelopment())
{
    // Включение Swagger для документации API
    app.UseSwagger();
    // Настройка Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API v1");
    });
}

// Миграции
using (var scope = app.Services.CreateScope())  // Область видимости
{
    // Получение инфы из контейнера
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Применение всех миграций к базе данных
    dbContext.Database.Migrate();
}

// Подключение аутентификации для проверки токенов
app.UseAuthentication();
// Подключение авторизации для проверки прав доступа
app.UseAuthorization();

// Настройка маршрутизации для контроллеров
app.MapControllers();

// Запуск приложения
app.Run();

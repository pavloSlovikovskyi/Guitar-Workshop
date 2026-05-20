using API.Extensions;
using API.Filters;
using Application;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System;

var builder = WebApplication.CreateBuilder(args);

// 1. Реєстрація сервісів шарів архітектури
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration);

// 2. Об'єднана в один чистий блок конфігурація контролерів, фільтрів та JSON
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ValidationExceptionFilter>();
})
.AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

// 3. Налаштування Swagger з підтримкою Bearer токенів
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Instrument Repair Workshop API", Version = "v1" });

    // Описуємо схему автентифікації для Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Будь ласка, введіть токен у форматі: Bearer {ваш_токен}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Робимо авторизацію обов'язковою для захищених ендпоінтів у UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 4. CORS політика
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

// 5. Реєстрація кастомних сервісів та валідаторів
builder.Services.AddScoped<IRepairOrderServiceTypeQueries, RepairOrderServiceTypeQueries>();
builder.Services.AddScoped<IRepairOrderServiceTypeRepository, RepairOrderServiceTypeRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
    await IdentityRoleSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Instrument Repair Workshop API v1");
        c.RoutePrefix = string.Empty; // Робить Swagger стартовою сторінкою (localhost:XXXX/)
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Важливо: спочатку автентифікація (хто ти), потім авторизація (що тобі можна)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
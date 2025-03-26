using Application;
using Persistence;
using Infrastructure;
using Persistence.Contexts.Data;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add Application, Infrastructure, Persistence services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerDocumentation();

// Exception middleware
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations and seed data
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration and seeding");
}

app.Run();
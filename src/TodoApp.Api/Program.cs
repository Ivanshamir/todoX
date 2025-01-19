using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

var currentDir = Directory.GetCurrentDirectory();
var solutionDir = Path.GetFullPath(Path.Combine(currentDir, ".."));

var envPath = Path.Combine(solutionDir, ".env");
Console.WriteLine($"Looking for .env at: {envPath}");
Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Automatically apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();

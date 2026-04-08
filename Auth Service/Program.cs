using Auth_Service.Business_handlers;
using Auth_Service.Data;
using Auth_Service.Interfaces;
using Auth_Service.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Generic Repository
//builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

// 3. UnitOfWork
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Register Serilog’s ILogger
builder.Services.AddSingleton(Log.Logger);

// Register repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();


// 4. MediatR
builder.Services.AddMediatR(typeof(RegisterUserHandler).Assembly);

// 5. Controllers
builder.Services.AddControllers();

// 6. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

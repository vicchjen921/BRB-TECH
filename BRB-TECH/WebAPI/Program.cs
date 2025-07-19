using Application.Repositories;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using HardCodeTest.Infrastructure.Repositories;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddDbContext<BrbTechDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BrbTechDbConnection"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BrbTechDbConnection"));
});

builder.Services
    .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddOptions<DocumentTransferConfig>().Bind(builder.Configuration.GetSection("DocTransferSettings"));
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddTransient<IDocumentTransferRepository, DocumentTransferRepository>();
builder.Services.AddTransient<IDocumentTransferService, DocumentTransferService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes((builder.Configuration.GetValue("Jwt:Key", "x2Fz9qA1JpR8tZs3UeD6MbY7VgW4nKc0"))))
        };
    });

PolicyRegister.AddPolicies(builder.Services);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHostedService<DocumentTransferHostedService>();

builder.Services.AddStackExchangeRedisCache(x => x.ConfigurationOptions = new ConfigurationOptions
{
    EndPoints = { builder.Configuration.GetConnectionString("RedisConnection") },
    Password = ""
});


builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("BrbTechDbConnection"));

var app = builder.Build();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

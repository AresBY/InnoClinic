using FluentValidation;
using FluentValidation.AspNetCore;
using InnoClinic.Authorization.Application.Features.Auth.Commands;
using InnoClinic.Authorization.Application.Interfaces;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.JWT;
using InnoClinic.Authorization.Domain.Entities;
using InnoClinic.Authorization.Infrastructure.Auth;
using InnoClinic.Authorization.Infrastructure.Email;
using InnoClinic.Authorization.Infrastructure.Email;
using InnoClinic.Authorization.Infrastructure.Persistence;
using InnoClinic.Authorization.Infrastructure.Persistence.Repositories;
using InnoClinic.Authorization.Infrastructure.Settings;
using InnoClinicCommon.Enums;
using InnoClinicCommon.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email:Gmail"));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = ClaimTypes.Role
    };
});

bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

if (IsDevelopment)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
}
else if (IsDocker)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
});
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommand>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddTransient<IEmailSender, GmailEmailSender>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();



if (IsDevelopment || IsDocker)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "InnoClinic API", Version = "v1" });

        // ��������� JWT �����������
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter the JWT token **without** the 'Bearer' prefix. Swagger will automatically add it.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

        c.EnableAnnotations();

        c.ExampleFilters();
    });

    // ������������ ��� ������� �� ������ � ���������
    builder.Services.AddSwaggerExamplesFromAssemblyOf<Application.Features.Auth.Examples.SignInCommandExample>();
}


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4300")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    bool deleted = await dbContext.Database.EnsureDeletedAsync();

//    if (deleted)
//    {
//        Console.WriteLine("Database was deleted successfully.");
//    }
//    else
//    {
//        Console.WriteLine("Database does not exist or could not be deleted.");
//    }
//}

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    dbContext.Database.Migrate();

    if (!dbContext.Users.Any())
    {
        var admin = new Admin
        {
            Email = "admin@example.com",
            Role = UserRole.Admin
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "adminadmin");

        dbContext.Users.Add(admin);
        dbContext.SaveChanges();
    }
}

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();


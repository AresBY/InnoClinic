using FluentValidation;
using FluentValidation.AspNetCore;
using InnoClinic.API.Middleware;
using InnoClinic.Application.JWT;
using InnoClinic.Infrastructure.Auth;
using InnoClinic.Infrastructure.Email;
using InnoClinic.Infrastructure.Settings;
using InnoClinic.Server.Application.Features.Auth.Commands;
using InnoClinic.Server.Application.Interfaces;
using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using InnoClinic.Server.Infrastructure.Email;
using InnoClinic.Server.Infrastructure.Persistence;
using InnoClinic.Server.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

if (IsDevelopment)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
}
else if(IsDocker)
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

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();



if (IsDevelopment || IsDocker)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
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
    dbContext.Database.Migrate();
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


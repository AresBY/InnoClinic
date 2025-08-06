using System.Text.Json.Serialization;

using Application.Features.Auth.Examples;

using FluentValidation;
using FluentValidation.AspNetCore;

using InnoClinic.Authorization.Application.Features.Auth.Commands.RegisterUser;
using InnoClinic.Authorization.Application.Interfaces;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.JWT;
using InnoClinic.Authorization.Domain.Entities;
using InnoClinic.Authorization.Infrastructure.Auth;
using InnoClinic.Authorization.Infrastructure.Email;
using InnoClinic.Authorization.Infrastructure.Persistence;
using InnoClinic.Authorization.Infrastructure.Persistence.Repositories;

using InnoClinicCommon.Enums;
using InnoClinicCommon.JWT;
using InnoClinicCommon.Middleware;
using InnoClinicCommon.Swagger;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

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
    SwaggerServiceExtensions.AddSwaggerWithJwt(builder.Services);

    builder.Services.AddSwaggerExamplesFromAssemblyOf<SignInCommandExample>();
    builder.Services.AddSwaggerGen(c => c.ExampleFilters());
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
        var receptionist = new Receptionist
        {
            Email = "Receptionist@mail.ru",
            Role = UserRole.Receptionist
        };

        receptionist.PasswordHash = passwordHasher.HashPassword(receptionist, "Receptionist");
        dbContext.Users.Add(receptionist);

        var patient = new Patient
        {
            Email = "Patient@mail.ru",
            Role = UserRole.Patient
        };

        patient.PasswordHash = passwordHasher.HashPassword(patient, "Patient");
        dbContext.Users.Add(patient);

        var doctor = new Doctor
        {
            Email = "Doctor@mail.ru",
            Role = UserRole.Doctor
        };

        doctor.PasswordHash = passwordHasher.HashPassword(doctor, "Doctor");
        dbContext.Users.Add(doctor);

        dbContext.SaveChanges();
    }
}

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

}
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();


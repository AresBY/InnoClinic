using FluentValidation;
using FluentValidation.AspNetCore;
using InnoClinic.API.Middleware;
using InnoClinic.Infrastructure.Email;
using InnoClinic.Server.Application.Common.Mappings;
using InnoClinic.Server.Application.Features.Auth.Commands;
using InnoClinic.Server.Application.Interfaces;
using InnoClinic.Server.Application.Interfaces.Repositories;
using InnoClinic.Server.Domain.Entities;
using InnoClinic.Server.Infrastructure.Email;
using InnoClinic.Server.Infrastructure.Persistence;
using InnoClinic.Server.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email:Gmail"));

//using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"))
//{
//    connection.Open();
//    using var command = connection.CreateCommand();
//    command.CommandText = @"
//        ALTER DATABASE [InnoClinicDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
//        DROP DATABASE [InnoClinicDb];
//    ";
//    command.ExecuteNonQuery();
//}

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
    cfg.RegisterServicesFromAssembly(typeof(RegisterPatientCommand).Assembly);
});
builder.Services.AddValidatorsFromAssemblyContaining<RegisterPatientCommand>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IPasswordHasher<Patient>, PasswordHasher<Patient>>();

builder.Services.AddTransient<IEmailSender, GmailEmailSender>();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


if (IsDevelopment || IsDocker)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular порт
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

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
    app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());

}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();


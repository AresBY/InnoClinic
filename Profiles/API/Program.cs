using FluentValidation;
using FluentValidation.AspNetCore;

using InnoClinic.Offices.Application.Features.Doctor.Commands.Examples;
using InnoClinic.Offices.Infrastructure.Persistence.Repositories;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Infrastructure.Persistence.Repositories;

using InnoClinicCommon.JWT;
using InnoClinicCommon.Middleware;
using InnoClinicCommon.Swagger;

using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// --- Environment Flags ---
bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

// --- Configuration ---
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (IsDevelopment)
{
    builder.Services.AddDbContext<ProfileDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
}
else if (IsDocker)
{
    builder.Services.AddDbContext<ProfileDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// --- JWT ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

// --- MediatR & FluentValidation ---
// Замените на свой Command из Profiles
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateDoctorProfileCommand).Assembly)
);
builder.Services.AddValidatorsFromAssembly(typeof(CreateDoctorProfileCommand).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// --- Infrastructure ---
// Если ты создашь метод AddInfrastructure в Profiles.Infrastructure
// builder.Services.AddInfrastructure(builder.Configuration);

// --- Controllers ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// --- Swagger ---
if (IsDevelopment || IsDocker)
{
    SwaggerServiceExtensions.AddSwaggerWithJwt(builder.Services);
    builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateDoctorCommandExample>();
    builder.Services.AddSwaggerGen(c => c.ExampleFilters());
}

// --- CORS ---
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
//    var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

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

// --- Middleware ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

    dbContext.Database.Migrate();
}

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

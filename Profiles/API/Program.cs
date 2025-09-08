using FluentValidation;
using FluentValidation.AspNetCore;

using InnoClinic.Offices.Infrastructure.Persistence.Repositories;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Features.Doctor.Examples;
using InnoClinic.Profiles.Application.Interfaces;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.StaticClases;
using InnoClinic.Profiles.Infrastructure.Persistence.Repositories;

using InnoClinicCommon.Middleware;
using InnoClinicCommon.Swagger;

using Microsoft.EntityFrameworkCore;

using Serilog;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// --- Environment Flags ---
bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

// --- Serilog ---
var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
if (!Directory.Exists(logPath))
    Directory.CreateDirectory(logPath);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(logPath, "log-.txt"), rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// --- Configuration ---
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddApplication();

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

// --- MediatR & FluentValidation ---
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateDoctorProfileCommand).Assembly)
);
builder.Services.AddValidatorsFromAssembly(typeof(CreateDoctorProfileCommand).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IDoctorProfileRepository, DoctorProfileRepository>();
builder.Services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
builder.Services.AddScoped<IReceptionistProfileRepository, ReceptionistProfileRepository>();




builder.Services.AddHttpClient<IOfficeApiClient, OfficeApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5180/");
});


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
//    await scope.ServiceProvider.GetRequiredService<ProfileDbContext>().Database.EnsureDeletedAsync();
//}

// --- Middleware ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

    dbContext.Database.Migrate();
    //if (!dbContext.Doctors.Any())
    //{
    //    var doctor = new DoctorProfile
    //    {
    //        OwnerId = new Guid("be0d5f6b-cc85-4477-96cc-28d37a4a3cc1"),
    //        FirstName = "John",
    //        LastName = "Doe",
    //        MiddleName = "Michael",
    //        Email = "johndoe@example.com",
    //        DateOfBirth = new DateTimeOffset(new DateTime(1985, 4, 23)),
    //        Specialization = DoctorSpecialization.Pediatrician,
    //        OfficeId = Guid.NewGuid(),
    //        CareerStartYear = 2010,
    //    };

    //    dbContext.Doctors.Add(doctor);

    //    dbContext.SaveChanges();
    //}
}

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors();


app.MapControllers();
app.Run();

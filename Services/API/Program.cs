using FluentValidation;
using FluentValidation.AspNetCore;

using InnoClinic.Services.Application.Features.Service.Examples;
using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Infrastructure.Persistence;
using InnoClinic.Services.Infrastructure.Repositories;

using InnoClinicCommon.JWT;
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

if (IsDevelopment)
{
    builder.Services.AddDbContext<ServicesDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
}
else if (IsDocker)
{
    builder.Services.AddDbContext<ServicesDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// --- JWT ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

// --- MediatR & FluentValidation ---
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateServiceCommand).Assembly)
);
builder.Services.AddValidatorsFromAssembly(typeof(CreateServiceCommandValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();


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
    builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateServiceCommandExample>();
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

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();



var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    await scope.ServiceProvider.GetRequiredService<ServicesDbContext>().Database.EnsureDeletedAsync();
//}

// --- Middleware ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ServicesDbContext>();

    dbContext.Database.Migrate();
}

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

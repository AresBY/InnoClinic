using GatewayAPI;

using InnoClinicCommon.JWT;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json and environment-specific file
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Generate Ocelot configuration
var rootPath = Directory.GetCurrentDirectory();
var configsPath = Path.Combine(rootPath, "Configs");
var outputFilePath = Path.Combine(rootPath, "ocelot.json");
OcelotConfigGenerator.Generate(configsPath, outputFilePath);
builder.Configuration.AddJsonFile(outputFilePath, optional: false, reloadOnChange: true);

// JWT authentication setup
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

// Polly DelegatingHandler for resilience
builder.Services.AddTransient<PollyDelegatingHandler>();

// Ocelot with Polly
builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<PollyDelegatingHandler>(true);

// Only SwaggerForOcelot for aggregated microservices
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddMvcCore().AddApiExplorer();

var app = builder.Build();

// Enable JWT authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Redirect root "/" to Swagger UI
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

// Swagger UI for aggregated microservices
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

// Ocelot middleware for routing
await app.UseOcelot();

app.Run();

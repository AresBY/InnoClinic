using GatewayAPI;

using InnoClinicCommon.JWT;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);


var rootPath = Directory.GetCurrentDirectory();

var configsPath = Path.Combine(rootPath, "Configs");
var outputFilePath = Path.Combine(rootPath, "ocelot.json");

OcelotConfigGenerator.Generate(configsPath, outputFilePath);

builder.Configuration.AddJsonFile(outputFilePath, optional: false, reloadOnChange: true);

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);


// Polly DelegatingHandler
builder.Services.AddTransient<PollyDelegatingHandler>();

// Ocelot + Polly
builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<PollyDelegatingHandler>(true);

// Swagger Aggregation
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});


// Swagger UI
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

// Ocelot Middleware
await app.UseOcelot();

app.Run();

using FluentValidation;
using FluentValidation.AspNetCore;

using InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice;
using InnoClinic.Offices.Infrastructure.Extensions;

using InnoClinicCommon.JWT;
using InnoClinicCommon.Middleware;
using InnoClinicCommon.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Application services
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOfficeCommand).Assembly)
);
builder.Services.AddValidatorsFromAssembly(typeof(CreateOfficeCommandValidator).Assembly);

bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtServiceExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger
if (IsDevelopment || IsDocker)
{
    SwaggerServiceExtensions.AddSwaggerWithJwt(builder.Services);
}


// Controllers
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddFluentValidationClientsideAdapters();

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

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

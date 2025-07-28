using FluentValidation;
using InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice;
using InnoClinic.Offices.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using InnoClinicCommon.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Application services
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOfficeCommand).Assembly)
);
builder.Services.AddValidatorsFromAssembly(typeof(CreateOfficeCommandValidator).Assembly);

bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (IsDevelopment || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

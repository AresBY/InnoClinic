using InnoClinic.Saga.Application.DTOs;
using InnoClinic.Saga.Application.Features.Doctor.Queries;
using InnoClinic.Saga.Contract;

using InnoClinicCommon.Swagger;

using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetDoctorsForReceptionistQuery).Assembly);
});

var isLocal = builder.Environment.IsDevelopment();

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<GetDoctorsForReceptionistRequest>();

    x.AddRequestClient<GetOfficeRequest>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(isLocal ? "localhost" : "rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMassTransitHostedService();



bool IsDevelopment = builder.Environment.IsEnvironment("Development");
bool IsDocker = builder.Environment.IsEnvironment("Docker");

// Swagger
if (IsDevelopment || IsDocker)
{
    SwaggerServiceExtensions.AddSwaggerWithJwt(builder.Services);
}

// -------------------- MVC --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// -------------------- Middleware --------------------
if (app.Environment.IsDevelopment() || IsDocker)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();

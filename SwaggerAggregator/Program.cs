using Ocelot.DependencyInjection;
using Ocelot.Middleware;

using SwaggerAggregator;

var builder = WebApplication.CreateBuilder(args);

OcelotConfigGenerator.Generate("services.json", "ocelot.generated.json");

builder.Configuration.AddJsonFile("ocelot.generated.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();


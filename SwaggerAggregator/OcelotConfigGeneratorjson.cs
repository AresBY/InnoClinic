using System.Text.Json;

namespace SwaggerAggregator
{
    public static class OcelotConfigGenerator
    {
        public static void Generate(string servicesFilePath, string outputFilePath)
        {
            var servicesJson = File.ReadAllText(servicesFilePath);
            var services = JsonSerializer.Deserialize<List<Service>>(servicesJson);

            if (services is null)
            {
                throw new InvalidOperationException("Не удалось десериализовать список сервисов из JSON.");
            }

            var routes = new List<object>();
            var swaggerEndpoints = new List<object>();

            foreach (var svc in services)
            {
                foreach (var controller in svc.Controllers)
                {
                    routes.Add(new
                    {
                        DownstreamPathTemplate = $"/api/{controller}/{{everything}}",
                        DownstreamScheme = "http",
                        DownstreamHostAndPorts = new[] { new { Host = "localhost", Port = svc.Port } },
                        UpstreamPathTemplate = $"/api/{controller}/{{everything}}",
                        UpstreamHttpMethod = new[] { "GET", "POST", "PUT", "DELETE", "PATCH" }
                    });
                }

                // Swagger route for each service
                routes.Add(new
                {
                    DownstreamPathTemplate = "/swagger/v1/swagger.json",
                    DownstreamScheme = "http",
                    DownstreamHostAndPorts = new[] { new { Host = "localhost", Port = svc.Port } },
                    UpstreamPathTemplate = $"/swagger/docs/{svc.Key}/swagger.json",
                    UpstreamHttpMethod = new[] { "GET" }
                });

                // Swagger UI aggregation
                swaggerEndpoints.Add(new
                {
                    Key = svc.Key,
                    TransformByOcelotConfig = false,
                    Config = new[]
                    {
                        new
                        {
                            Name = svc.Name,
                            Version = "v1",
                            Url = $"http://localhost:{svc.Port}/swagger/v1/swagger.json"
                        }
                    }
                });
            }

            var ocelotConfig = new
            {
                Routes = routes,
                SwaggerEndPoints = swaggerEndpoints,
                GlobalConfiguration = new { BaseUrl = "http://localhost:5000" }
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var ocelotJson = JsonSerializer.Serialize(ocelotConfig, options);
            File.WriteAllText(outputFilePath, ocelotJson);
        }
    }
}

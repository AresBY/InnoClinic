using System.Text.Json;
using System.Text.Json.Serialization;

using GatewayAPI.Entities.OcelotConfigGenerator;

namespace GatewayAPI
{
    public static class OcelotConfigGenerator
    {
        private static string RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        // Generates Ocelot configuration for all microservices under a root folder
        public static void Generate(string configsRootPath, string outputFilePath)
        {
            if (!Directory.Exists(configsRootPath))
                throw new DirectoryNotFoundException($"Config folder not found: {configsRootPath}");

            var microserviceDirs = Directory.GetDirectories(configsRootPath);
            var routes = new List<object>();
            var swaggerEndpoints = new List<object>();
            var microservices = new List<MicroserviceView>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            bool useDockerHosts = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            foreach (var msDir in microserviceDirs)
            {
                var msName = Path.GetFileName(msDir);
                var controllerFiles = Directory.GetFiles(msDir, "*.json");
                var controllers = new List<ControllerView>();
                int port = 0;
                string baseUrl = null;

                foreach (var file in controllerFiles)
                {
                    var json = File.ReadAllText(file);
                    var ctrlConfig = JsonSerializer.Deserialize<ControllerConfig>(json, options);
                    if (ctrlConfig == null) continue;

                    if (baseUrl == null) baseUrl = ctrlConfig.BaseUrl;
                    if (port == 0) port = ctrlConfig.Port;

                    controllers.Add(new ControllerView
                    {
                        Controller = ctrlConfig.Name,
                        Endpoints = ctrlConfig.Endpoints
                    });

                    foreach (var ep in ctrlConfig.Endpoints)
                    {
                        string hostName = useDockerHosts ? msName.ToLower() : "localhost";
                        int containerPort = useDockerHosts ? 80 : port;

                        var route = new Dictionary<string, object?>
                        {
                            ["DownstreamPathTemplate"] = ep.Path,
                            ["DownstreamScheme"] = "http",
                            ["DownstreamHostAndPorts"] = new[] { new { Host = hostName, Port = containerPort } },
                            ["UpstreamPathTemplate"] = ep.Path,
                            ["UpstreamHttpMethod"] = new[] { ep.Method.ToUpper() }
                        };

                        if (ep.Roles.Count > 0)
                        {
                            route["AuthenticationOptions"] = new
                            {
                                AuthenticationProviderKey = "Bearer",
                                AllowedScopes = Array.Empty<string>()
                            };

                            route["RouteClaimsRequirement"] = new Dictionary<string, string>
                            {
                                { RoleClaim, string.Join(",", ep.Roles) }
                            };
                        }

                        routes.Add(route);
                    }
                }

                // Swagger endpoint config
                if (baseUrl != null)
                {
                    string swaggerUrl = useDockerHosts
                        ? $"http://{msName.ToLower()}:80/swagger/v1/swagger.json"
                        : $"{baseUrl}/swagger/v1/swagger.json";

                    swaggerEndpoints.Add(new
                    {
                        Key = msName.ToLower(),
                        TransformByOcelotConfig = false,
                        Config = new[]
                        {
                            new
                            {
                                Name = msName + " API",
                                Version = "v1",
                                Url = swaggerUrl
                            }
                        }
                    });
                }

                microservices.Add(new MicroserviceView
                {
                    Microservice = msName,
                    Controllers = controllers
                });
            }

            // Final Ocelot config object
            var ocelotConfig = new
            {
                Routes = routes,
                SwaggerEndPoints = swaggerEndpoints,
                Microservices = microservices,
                GlobalConfiguration = new { BaseUrl = "http://localhost:5000" }
            };

            // Serialize and write to output file
            var outputJson = JsonSerializer.Serialize(ocelotConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(outputFilePath, outputJson);
        }
    }
}

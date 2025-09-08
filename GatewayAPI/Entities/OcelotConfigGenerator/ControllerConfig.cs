namespace GatewayAPI.Entities.OcelotConfigGenerator
{
    public class ControllerConfig
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public int Port { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new();
    }
}

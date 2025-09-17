namespace GatewayAPI.Entities.OcelotConfigGenerator
{
    public class Service
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new();
        public int Port { get; set; }
    }
}

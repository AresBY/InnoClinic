namespace GatewayAPI.Entities.OcelotConfigGenerator
{
    public class ControllerView
    {
        public string Controller { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new();
    }
}

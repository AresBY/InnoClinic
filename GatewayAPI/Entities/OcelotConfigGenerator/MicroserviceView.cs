namespace GatewayAPI.Entities.OcelotConfigGenerator
{
    public class MicroserviceView
    {
        public string Microservice { get; set; }
        public List<ControllerView> Controllers { get; set; } = new();
    }
}

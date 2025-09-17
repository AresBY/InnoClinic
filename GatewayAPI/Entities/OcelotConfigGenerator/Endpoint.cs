using InnoClinicCommon.Enums;

namespace GatewayAPI.Entities.OcelotConfigGenerator
{
    public class Endpoint
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public List<UserRole> Roles { get; set; } = new();
    }
}

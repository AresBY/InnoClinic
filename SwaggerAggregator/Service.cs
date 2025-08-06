namespace SwaggerAggregator
{
    public class Service
    {
        public string Key { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Port { get; set; }
        public List<string> Controllers { get; set; } = new();
    }
}

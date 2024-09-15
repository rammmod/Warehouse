namespace Warehouse.IntegrationTests.Infrastructure
{
    public class InMemoryStaticConfiguration
    {
        public static Dictionary<string, string> Configuration { get; } = new();
    }
}

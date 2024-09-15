using Microsoft.Extensions.Configuration;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public static class ApplicationConfiguration
    {
        public static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.Testing.json", false)
                .Add(new DynamicMemoryConfigurationSource { InitialData = InMemoryStaticConfiguration.Configuration })
                .Build();

        public static T Get<T>(string prefix = null)
        {
            var section = Configuration;
            if (!string.IsNullOrEmpty(prefix))
                section = Configuration.GetSection(prefix);
            return section.Get<T>();
        }
    }
}

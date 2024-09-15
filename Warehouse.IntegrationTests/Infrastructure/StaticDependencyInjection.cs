using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class StaticDependencyInjection
    {
        private static Lazy<IServiceProvider> _serviceProvider = new(() => ServiceCollection.BuildServiceProvider());
        public static IServiceCollection ServiceCollection { get; } = CreateServiceCollection();
        public static IServiceProvider ServiceProvider => _serviceProvider.Value;

        public static void Clean()
        {
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            ServiceCollection.Clear();
            ServiceCollection.AddSingleton(ApplicationConfiguration.Configuration);

            _serviceProvider = new Lazy<IServiceProvider>(() => ServiceCollection.BuildServiceProvider());
        }

        private static IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddSingleton(ApplicationConfiguration.Configuration);
            return services;
        }
    }
}

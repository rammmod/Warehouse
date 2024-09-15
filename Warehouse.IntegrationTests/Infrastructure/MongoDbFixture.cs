using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Repositories;
using WarehouseApi.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class MongoDbFixture
    {
        private IServiceProvider ServiceProvider { get; }
        public MongoDbFixture()
        {
            var services = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.Testing.json")
                .Build();

            var mongoDbConfiguration = configuration.GetSection(nameof(MongoDbConfiguration)).Get<MongoDbConfiguration>();

            services.TryAddSingleton(mongoDbConfiguration);

            services.TryAddSingleton<MongoDbClientFactory>();
            services.TryAddSingleton(p => p.GetRequiredService<MongoDbClientFactory>().Get());
            services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            services.TryAddSingleton<ICategoryRepository, CategoryRepository>();
            services.TryAddSingleton<IProductRepository, ProductRepository>();
            services.TryAddSingleton<IOrderRepository, OrderRepository>();

            ServiceProvider = services.BuildServiceProvider();

            services.AddSingleton(ApplicationConfiguration.Configuration);

            foreach (var serviceDescriptor in services)
            {
                StaticDependencyInjection.ServiceCollection.Add(serviceDescriptor);
            }

        }

        public IMongoDbContext MongoDbContext => ServiceProvider.GetRequiredService<IMongoDbContext>();

        public IMongoCollection<T> GetCollection<T>()
        {
            var collection = MongoDbContext.GetCollection<T>();
            return collection;
        }

        public async Task<T> InsertAsync<T>(T item)
        {
            var collection = GetCollection<T>();
            await collection.InsertOneAsync(item);
            return item;
        }

        public void PurgeAll()
        {
            PurgeAllAsync().GetAwaiter().GetResult();
        }

        public async Task PurgeAllAsync()
        {
            var client = MongoDbContext.GetCollection<object>().Database;
            var names = await (await client.ListCollectionNamesAsync()).ToListAsync();
            foreach (var name in names)
            {
                await client.DropCollectionAsync(name);
            }
        }
    }
}

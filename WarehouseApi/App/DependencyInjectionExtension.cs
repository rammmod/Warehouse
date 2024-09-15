using FluentValidation;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Repositories;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Sagas;
using WarehouseApi.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using MongoDbContext = Warehouse.Infrastructure.MongoDbContext;

namespace WarehouseApi.App
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetEntryAssembly());

            var mongoDbConfiguration = configuration.GetSection(nameof(MongoDbConfiguration)).Get<MongoDbConfiguration>();
            var massTransitConfiguration = configuration.GetSection(nameof(MassTransitConfiguration)).Get<MassTransitConfiguration>();

            services.TryAddSingleton(mongoDbConfiguration);

            services.TryAddSingleton<MongoDbClientFactory>();
            services.TryAddSingleton(p => p.GetRequiredService<MongoDbClientFactory>().Get());
            services.TryAddSingleton<IMongoDbContext, MongoDbContext>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetEntryAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());

            services.TryAddTransient<IStockService, StockService>();

            services.TryAddSingleton<ICategoryRepository, CategoryRepository>();
            services.TryAddSingleton<IProductRepository, ProductRepository>();
            services.TryAddSingleton<IOrderRepository, OrderRepository>();

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddDelayedMessageScheduler();

                x.AddConsumers(Assembly.GetEntryAssembly());

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoStart = true;
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseMessageRetry(x => x.Interval(5, TimeSpan.FromSeconds(2)));

                    cfg.Host(massTransitConfiguration.Host, massTransitConfiguration.Port, massTransitConfiguration.VirtualHost, h =>
                    {
                        h.Username(massTransitConfiguration.Username);
                        h.Password(massTransitConfiguration.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });

                x.AddSagaStateMachine<CreateOrderSaga, OrderSagaState>()
                .MongoDbRepository(r =>
                {
                    r.Connection = mongoDbConfiguration.ConnectionString;
                    r.DatabaseName = mongoDbConfiguration.DatabaseName;
                    r.CollectionName = "OrdersSaga";
                });
            });
        }
    }
}

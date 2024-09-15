using WarehouseApi.Contracts;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Enums;
using MassTransit;

namespace WarehouseApi.Consumers
{
    public class ProductStockChangedConsumer : IConsumer<ProductStockChangedContract>
    {
        readonly IOrderRepository _orderRepository;
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IPublishEndpoint _publishEndpoint;

        public ProductStockChangedConsumer(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProductStockChangedContract> context)
        {
            var product = await _productRepository.GetAsync(context.Message.ProductId, context.CancellationToken);
            var category = await _categoryRepository.GetAsync(product.CategoryId, context.CancellationToken);

            var order = (await _orderRepository.GetAllAsync(context.CancellationToken))
                .FirstOrDefault(m => m.Status == OrderStatusEnum.Pending &&
                                m.ProductId == product.Id
                                && product.Stock - m.Quantity > category.OutOfStock);

            if (order is not null)
            {
                var createOrderContract = new CreateOrderContract()
                {
                    CorrelationId = Guid.NewGuid(),
                    Order = order,
                    Mode = OutOfStockModeEnum.ReserveWhenAvailable
                };

                await _publishEndpoint.Publish(createOrderContract, context.CancellationToken);
            }
        }
    }
}

using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Sagas.Contracts;
using WarehouseApi.Services;
using MassTransit;

namespace WarehouseApi.Sagas.Consumers
{
    public class RejectOrderCommandConsumer : IConsumer<RejectOrderCommand>
    {
        readonly IProductRepository _productRepository;
        readonly IOrderRepository _orderRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IStockService _stockService;

        public RejectOrderCommandConsumer(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICategoryRepository categoryRepository,
            IStockService stockService)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _stockService = stockService;
        }

        public async Task Consume(ConsumeContext<RejectOrderCommand> context)
        {
            var order = await _orderRepository.GetAsync(context.Message.OrderId, context.CancellationToken);

            var product = await _productRepository.GetAsync(order.ProductId, context.CancellationToken);

            product.Stock += order.Quantity;
            order.Status = OrderStatusEnum.Rejected;

            await _stockService.UpdateStockAndCheckForChange(product, context.CancellationToken);

            await _orderRepository.ProceedAsync(order, context.CancellationToken);
        }
    }
}

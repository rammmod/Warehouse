using WarehouseApi.Contracts;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Enums;
using WarehouseApi.Exceptions;
using WarehouseApi.Sagas.Contracts;
using WarehouseApi.Services;
using MassTransit;

namespace WarehouseApi.Consumers
{
    public class CreateOrderConsumer : IConsumer<CreateOrderContract>
    {
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IOrderRepository _orderRepository;
        readonly IStockService _stockService;
        readonly IPublishEndpoint _publishEndpoint;

        public CreateOrderConsumer(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository,
            IStockService stockService,
            IPublishEndpoint publishEndpoint)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _stockService = stockService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CreateOrderContract> context)
        {
            (var remainingStock, var product, var category) = await GetStockAsync(context.Message.Order, context.CancellationToken);

            var createOrderResponse = new CreateOrderResponse();

            if (remainingStock > category.LowStock)
            {
                context.Message.Order.Status = OrderStatusEnum.Approved;
            }
            else if (remainingStock <= category.LowStock && remainingStock > category.OutOfStock)
            {
                context.Message.Order.Status = OrderStatusEnum.UnderReview;
            }
            else if (context.Message.Mode == OutOfStockModeEnum.None)
            {
                createOrderResponse.Error = "Unable to order";
            }

            await CreateOrder(remainingStock, context.Message.Order, product, context.CancellationToken);

            await context.RespondAsync(createOrderResponse);
        }

        private async Task<(int, Product, Category)> GetStockAsync(Order request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken) ?? throw new ProductNotFoundException();

            var category = await _categoryRepository.GetAsync(product.CategoryId, cancellationToken);

            var remainingStock = product.Stock - request.Quantity;

            return (remainingStock, product, category);
        }

        private async Task CreateOrder(int remainingStock, Order order, Product product, CancellationToken cancellationToken)
        {
            product.Stock = remainingStock;

            if (order.Status is not OrderStatusEnum.Pending)
                await _orderRepository.ProceedAsync(order, cancellationToken);

            if (order.Status is OrderStatusEnum.UnderReview)
                await CreateOrderSagaAsync(order, cancellationToken);

            if (order.Status is not OrderStatusEnum.Pending)
                await _stockService.UpdateStockAndCheckForChange(product, cancellationToken);
        }

        private async Task CreateOrderSagaAsync(Order request, CancellationToken cancellationToken)
        {
            var createOrderSagaCommand = new CreateOrderSagaCommand()
            {
                CorrelationId = Guid.NewGuid(),
                OrderId = request.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _publishEndpoint.Publish(createOrderSagaCommand, cancellationToken);
        }
    }
}

using FluentValidation;
using WarehouseApi.Commands.Orders;
using WarehouseApi.Contracts;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Order;
using MassTransit;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        readonly IValidator<CreateOrderRequest> _validator;
        readonly IProductRepository _productRepository;
        readonly IOrderRepository _orderRepository;
        readonly IRequestClient<CreateOrderContract> _request;

        public CreateOrderCommandHandler(
            IValidator<CreateOrderRequest> validator,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IRequestClient<CreateOrderContract> request)
        {
            _validator = validator;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _request = request;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.order, cancellationToken);

            _ = await _productRepository.GetAsync(request.order.ProductId, cancellationToken) ?? throw new ProductNotFoundException();

            var order = new Order()
            {
                ProductId = request.order.ProductId,
                Quantity = request.order.Quantity,
                Status = OrderStatusEnum.Pending
            };

            order = await _orderRepository.AddAsync(order, cancellationToken);

            var result = await _request.GetResponse<CreateOrderResponse>(new CreateOrderContract()
            {
                CorrelationId = Guid.NewGuid(),
                Order = order,
                Mode = request.order.Mode
            });

            if (result.Message.Error is not null)
                throw new UnableToOrderException();

            return Unit.Value;
        }
    }
}

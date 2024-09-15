using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Sagas.Contracts;
using MassTransit;

namespace WarehouseApi.Sagas.Consumers
{
    public class ApproveOrderCommandConsumer : IConsumer<ApproveOrderCommand>
    {
        readonly IOrderRepository _orderRepository;
        public ApproveOrderCommandConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<ApproveOrderCommand> context)
        {
            var order = new Order()
            {
                Id = context.Message.OrderId,
                Status = OrderStatusEnum.Approved
            };

            await _orderRepository.ProceedAsync(order, context.CancellationToken);
        }
    }
}

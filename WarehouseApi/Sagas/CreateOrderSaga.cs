using WarehouseApi.Domain.Enums;
using WarehouseApi.Sagas.Contracts;
using MassTransit;

namespace WarehouseApi.Sagas
{
    public class CreateOrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        public Event<CreateOrderSagaCommand> CreateOrderCommand { get; }
        public Event<ApproveOrderResponseCommand> ApproveOrderResponseCommandEvent { get; }

        public State UnderReview { get; }

        public CreateOrderSaga()
        {
            InstanceState(c => c.CurrentState);

            Initially(
                When(CreateOrderCommand)
                    .Then(x =>
                    {
                        x.Saga.CorrelationId = x.Message.CorrelationId;
                        x.Saga.OrderId = x.Message.OrderId;
                        x.Saga.ProductId = x.Message.ProductId;
                        x.Saga.Quantity = x.Message.Quantity;
                        x.Saga.Status = OrderStatusEnum.UnderReview;
                    })
                    .Publish(x => ApproveOrderRequestCommand(x.Saga))
                    .TransitionTo(UnderReview)
            );

            During(UnderReview,
                When(ApproveOrderResponseCommandEvent)
                .IfElse(x => x.Message.IsApproved,
                    x => x.Then(x =>
                        {
                            x.Saga.Status = OrderStatusEnum.Approved;
                        })
                        .Publish(x => ApproveOrderCommand(x.Saga))
                        .TransitionTo(Final),
                    x => x.Then(x =>
                        {
                            x.Saga.Status = OrderStatusEnum.Rejected;
                        })
                        .Publish(x => RejectOrderCommand(x.Saga))
                        .TransitionTo(Final))
            );
        }

        private static ApproveOrderRequestCommand ApproveOrderRequestCommand(OrderSagaState saga)
        {
            return new ApproveOrderRequestCommand()
            {
                CorrelationId = saga.CorrelationId,
                OrderId = saga.OrderId,
            };
        }

        private static ApproveOrderCommand ApproveOrderCommand(OrderSagaState saga)
        {
            return new ApproveOrderCommand()
            {
                CorrelationId = saga.CorrelationId,
                OrderId = saga.OrderId,
            };
        }

        private static RejectOrderCommand RejectOrderCommand(OrderSagaState saga)
        {
            return new RejectOrderCommand()
            {
                CorrelationId = saga.CorrelationId,
                OrderId = saga.OrderId,
            };
        }
    }
}

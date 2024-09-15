using WarehouseApi.Domain.Enums;
using MassTransit;
using MongoDB.Bson.Serialization.Attributes;

namespace WarehouseApi.Sagas
{
    public class OrderSagaState : SagaStateMachineInstance, ISagaVersion
    {
        [BsonId]
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string CurrentState { get; set; }
    }
}

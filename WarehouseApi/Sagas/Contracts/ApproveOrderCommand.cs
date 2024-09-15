using MassTransit;

namespace WarehouseApi.Sagas.Contracts
{
    public class ApproveOrderCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
    }
}

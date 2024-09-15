using MassTransit;

namespace WarehouseApi.Sagas.Contracts
{
    public class ApproveOrderResponseCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public bool IsApproved { get; set; }
    }
}

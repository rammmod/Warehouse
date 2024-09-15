using WarehouseApi.Sagas.Contracts;
using MassTransit;

namespace WarehouseApi.Sagas.Consumers
{
    public class ApproveOrderRequestCommandConsumer : IConsumer<ApproveOrderRequestCommand>
    {
        readonly IPublishEndpoint _endpoint;
        public ApproveOrderRequestCommandConsumer(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Consume(ConsumeContext<ApproveOrderRequestCommand> context)
        {
            Random random = new();
            int randomNumber = random.Next(0, 2);

            var contract = new ApproveOrderResponseCommand()
            {
                CorrelationId = context.Message.CorrelationId,
                IsApproved = randomNumber != 0
            };

            Thread.Sleep(1000);

            await _endpoint.Publish(contract, context.CancellationToken);
        }
    }
}

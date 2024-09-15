using AutoFixture;
using FluentAssertions;
using Warehouse.IntegrationTests.Infrastructure;
using WarehouseApi.Domain.Enums;
using WarehouseApi.Sagas;
using WarehouseApi.Sagas.Contracts;
using MongoDB.Driver;
using System.Diagnostics;

namespace Warehouse.IntegrationTests.Sagas
{
    public class CreateOrderSagaTest : IClassFixture<AutoDataFixture>,
        IClassFixture<MongoDbFixture>,
        IClassFixture<SagaFixture<CreateOrderSaga, OrderSagaState>>,
        IDisposable
    {
        public CreateOrderSagaTest(
            AutoDataFixture autoDataFixture,
            MongoDbFixture mongoDbFixture,
            SagaFixture<CreateOrderSaga, OrderSagaState> sagaFixture)
        {
            AutoDataFixture = autoDataFixture;
            MongoDbFixture = mongoDbFixture;
            SagaFixture = sagaFixture;

            SagaFixture.StartHarness("OrderSagaState");
            CancellationTokenSource = new CancellationTokenSource();
        }

        public AutoDataFixture AutoDataFixture { get; set; }

        public MongoDbFixture MongoDbFixture { get; set; }

        public SagaFixture<CreateOrderSaga, OrderSagaState> SagaFixture { get; set; }

        public IFixture AutoFixture => AutoDataFixture.AutoFixture;

        public CancellationTokenSource CancellationTokenSource { get; set; }


        [Fact]
        public async void When_CreateOrderSagaCommand_Should_Start_Saga_And_Publish_ApproveOrderRequestCommand()
        {
            // arrange
            var harness = SagaFixture.TestHarness.Harness;
            var sagaHarness = SagaFixture.TestHarness.SagaHarness;
            var correlationId = AutoFixture.Create<Guid>();

            var startSagaCommand = AutoFixture.Build<CreateOrderSagaCommand>()
                .With(x => x.CorrelationId, correlationId)
                .With(x => x.ProductId, 1)
                .With(x => x.Quantity, 1)
                .With(x => x.OrderId, 1)
                .Create();

            // act
            await harness.Bus.Publish(startSagaCommand, UseToken());
            await sagaHarness.Exists(startSagaCommand.CorrelationId);

            // assert
            var isPublished = await harness.Published.Any<CreateOrderSagaCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);
            var isConsumed = await harness.Consumed.Any<CreateOrderSagaCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);

            var publishApproveOrderRequestCommand = harness.Published
                .Select<ApproveOrderRequestCommand>(f => f.Context.Message.CorrelationId == correlationId, UseToken())
                .FirstOrDefault();

            isPublished.Should().BeTrue();
            isConsumed.Should().BeTrue();
            publishApproveOrderRequestCommand.Should().NotBeNull();

            var expectedApproveOrderRequestCommand = AutoFixture.Build<ApproveOrderRequestCommand>()
                .With(x => x.CorrelationId, startSagaCommand.CorrelationId)
                .With(x => x.OrderId, startSagaCommand.OrderId)
                .Create();

            publishApproveOrderRequestCommand.Context.Message.Should().BeEquivalentTo(expectedApproveOrderRequestCommand);

            var sagaInstance = await MongoDbFixture.GetCollection<OrderSagaState>()
                .Find(x => x.CorrelationId == correlationId)
                .FirstOrDefaultAsync(UseToken());

            var expectedSagaInstance = new OrderSagaState
            {
                Version = 0,
                CurrentState = nameof(OrderStatusEnum.UnderReview),
                CorrelationId = correlationId,
                OrderId = startSagaCommand.OrderId,
                ProductId = startSagaCommand.ProductId,
                Quantity = startSagaCommand.Quantity,
                Status = OrderStatusEnum.UnderReview
            };

            sagaInstance.Should().BeEquivalentTo(expectedSagaInstance);
        }

        [Fact]
        public async void When_ApproveOrderRequestCommandComplete_And_Answer_Is_True_Should_Publish_ApproveOrderCommand()
        {
            // arrange
            var harness = SagaFixture.TestHarness.Harness;
            var sagaHarness = SagaFixture.TestHarness.SagaHarness;
            var correlationId = AutoFixture.Create<Guid>();

            var sagaState = AutoFixture.Build<OrderSagaState>()
                .With(x => x.CorrelationId, correlationId)
                .With(x => x.OrderId, 1)
                .With(x => x.Quantity, 1)
                .With(x => x.ProductId, 1)
                .With(x => x.Status, OrderStatusEnum.UnderReview)
                .With(x => x.CurrentState, nameof(OrderStatusEnum.UnderReview))
                .With(x => x.Version, 0)
                .Create();

            var approveOrderResponseCommand = AutoFixture.Build<ApproveOrderResponseCommand>()
                .With(x => x.CorrelationId, correlationId)
                .With(x => x.IsApproved, true)
                .Create();

            await MongoDbFixture.InsertAsync(sagaState);

            // act
            await harness.Bus.Publish(approveOrderResponseCommand, UseToken());
            await sagaHarness.Exists(sagaState.CorrelationId);

            // assert
            var isPublished = await harness.Published.Any<ApproveOrderResponseCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);

            var approveOrderCommand = harness.Published
                .Select<ApproveOrderCommand>(f => f.Context.Message.CorrelationId == correlationId, UseToken())
                .FirstOrDefault();

            isPublished.Should().BeTrue();

            Thread.Sleep(3000);

            var isConsumed = await harness.Consumed.Any<ApproveOrderResponseCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);

            isConsumed.Should().BeTrue();
            approveOrderCommand.Should().NotBeNull();

            var sagaInstance = await MongoDbFixture.GetCollection<OrderSagaState>()
                .Find(x => x.CorrelationId == correlationId)
                .FirstOrDefaultAsync(UseToken());

            var expectedApproveOrderCommand = AutoFixture.Build<ApproveOrderCommand>()
                .With(x => x.CorrelationId, sagaInstance.CorrelationId)
                .With(x => x.OrderId, sagaInstance.OrderId)
                .Create();

            approveOrderCommand.Context.Message.Should().BeEquivalentTo(expectedApproveOrderCommand);

            var expectedSagaInstance = new OrderSagaState
            {
                Version = sagaState.Version + 1,
                CurrentState = "Final",
                CorrelationId = correlationId,
                OrderId = sagaState.OrderId,
                ProductId = sagaState.ProductId,
                Quantity = sagaState.Quantity,
                Status = OrderStatusEnum.Approved
            };

            sagaInstance.Should().BeEquivalentTo(expectedSagaInstance);
        }

        [Fact]
        public async void When_ApproveOrderRequestCommandComplete_And_Answer_Is_False_Should_Publish_RejectOrderCommand()
        {
            // arrange
            var harness = SagaFixture.TestHarness.Harness;
            var sagaHarness = SagaFixture.TestHarness.SagaHarness;
            var correlationId = AutoFixture.Create<Guid>();

            var sagaState = AutoFixture.Build<OrderSagaState>()
                .With(x => x.CorrelationId, correlationId)
                .With(x => x.OrderId, 1)
                .With(x => x.Quantity, 1)
                .With(x => x.ProductId, 1)
                .With(x => x.Status, OrderStatusEnum.UnderReview)
                .With(x => x.CurrentState, nameof(OrderStatusEnum.UnderReview))
                .With(x => x.Version, 0)
                .Create();

            var approveOrderResponseCommand = AutoFixture.Build<ApproveOrderResponseCommand>()
                .With(x => x.CorrelationId, correlationId)
                .With(x => x.IsApproved, false)
                .Create();

            await MongoDbFixture.InsertAsync(sagaState);

            // act
            await harness.Bus.Publish(approveOrderResponseCommand, UseToken());
            await sagaHarness.Exists(sagaState.CorrelationId);

            // assert
            var isPublished = await harness.Published.Any<ApproveOrderResponseCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);

            var rejectOrderCommand = harness.Published
                .Select<RejectOrderCommand>(f => f.Context.Message.CorrelationId == correlationId, UseToken())
                .FirstOrDefault();

            isPublished.Should().BeTrue();

            Thread.Sleep(3000);

            var isConsumed = await harness.Consumed.Any<ApproveOrderResponseCommand>(x =>
                x.Context.Message.CorrelationId == correlationId);

            isConsumed.Should().BeTrue();
            rejectOrderCommand.Should().NotBeNull();

            var sagaInstance = await MongoDbFixture.GetCollection<OrderSagaState>()
                .Find(x => x.CorrelationId == correlationId)
                .FirstOrDefaultAsync(UseToken());

            var expectedRejectOrderCommand = AutoFixture.Build<ApproveOrderCommand>()
                .With(x => x.CorrelationId, sagaInstance.CorrelationId)
                .With(x => x.OrderId, sagaInstance.OrderId)
                .Create();

            rejectOrderCommand.Context.Message.Should().BeEquivalentTo(expectedRejectOrderCommand);

            var expectedSagaInstance = new OrderSagaState
            {
                Version = sagaState.Version + 1,
                CurrentState = "Final",
                CorrelationId = correlationId,
                OrderId = sagaState.OrderId,
                ProductId = sagaState.ProductId,
                Quantity = sagaState.Quantity,
                Status = OrderStatusEnum.Rejected
            };

            sagaInstance.Should().BeEquivalentTo(expectedSagaInstance);
        }

        public CancellationToken UseToken(TimeSpan timeSpan = default)
        {
            if (!Debugger.IsAttached)
                CancellationTokenSource.CancelAfter(timeSpan == default ? TimeSpan.FromSeconds(4) : timeSpan);

            return CancellationTokenSource.Token;
        }

        public virtual void Dispose()
        {
            MongoDbFixture.PurgeAll();
        }
    }
}

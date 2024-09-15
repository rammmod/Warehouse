using MassTransit;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.Testing;
using MassTransit.Testing.Implementations;
using Microsoft.Extensions.Configuration;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class SagaFixture<TInstance, TState>
        where TInstance : MassTransitStateMachine<TState>, new()
        where TState : class, SagaStateMachineInstance, ISagaVersion
    {
        private string _collectionName;

        public SagaFixture()
        {
            RabbitMqTestHarnessLazy = new Lazy<SagaTestHarness<TInstance, TState>>(() =>
            {
                var harness = new InMemoryTestHarness();

                var mongoDbConnectionString =
                    ApplicationConfiguration.Configuration.GetValue<string>("MongoDbConfiguration:ConnectionString");
                var mongoDbDatabaseName = ApplicationConfiguration.Configuration.GetValue<string>("MongoDbConfiguration:DatabaseName");
                var repository = MongoDbSagaRepository<TState>
                    .Create(mongoDbConnectionString, mongoDbDatabaseName, _collectionName ?? typeof(TState).Name);
                var sagaTestHarness = harness.StateMachineSaga(new TInstance(), repository);

                harness.TestTimeout = TimeSpan.FromMilliseconds(500);
                harness.Start().GetAwaiter().GetResult();

                return new SagaTestHarness<TInstance, TState>(harness, (StateMachineSagaTestHarness<TState, TInstance>)sagaTestHarness);
            });
        }

        private Lazy<SagaTestHarness<TInstance, TState>> RabbitMqTestHarnessLazy { get; }

        public SagaTestHarness<TInstance, TState> TestHarness => RabbitMqTestHarnessLazy.Value;

        public SagaTestHarness<TInstance, TState> StartHarness(string collectionName = null)
        {
            _collectionName = collectionName;
            return RabbitMqTestHarnessLazy.Value;
        }
    }

    public class SagaTestHarness<TInstance, TState>
        where TInstance : MassTransitStateMachine<TState>, new()
        where TState : class, SagaStateMachineInstance, ISagaVersion
    {
        public SagaTestHarness(
            InMemoryTestHarness harness,
            StateMachineSagaTestHarness<TState, TInstance> sagaHarness)
        {
            Harness = harness;
            SagaHarness = sagaHarness;
        }

        public InMemoryTestHarness Harness { get; }

        public StateMachineSagaTestHarness<TState, TInstance> SagaHarness { get; }
    }
}

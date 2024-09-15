using AutoFixture;
using AutoFixture.Dsl;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class AutoDataFixture
    {
        public IFixture AutoFixture { get; }

        public AutoDataFixture()
        {
            AutoFixture = new Fixture();

        }
    }
}

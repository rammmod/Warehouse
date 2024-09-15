using AutoFixture;
using AutoFixture.AutoMoq;

namespace Warehouse.UnitTests.Fixtures
{
    public class BaseFixture
    {
        public IFixture BuildFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            return fixture;
        }
    }
}

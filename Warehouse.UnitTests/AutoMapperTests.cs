using AutoMapper;
using WarehouseApi.App.AutoMapperProfiles;

namespace WarehouseApi.UnitTests
{
    public class AutoMapperTests
    {
        [Fact]
        public void When_MappingProfile_Should_Be_Valid()
        {
            var config = new MapperConfiguration(expression =>
            {
                expression.AddProfile<AutoMapperProfiles>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}

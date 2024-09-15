using AutoMapper;
using WarehouseApi;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.UnitTests.Fixtures
{
    public class MapperFixture
    {
        public MapperFixture()
        {
            Mapper = new ServiceCollection()
                .AddAutoMapper(typeof(Program).Assembly)
                .BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }

        public IMapper Mapper { get; }
    }
}

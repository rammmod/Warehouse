using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Models.Order;

namespace Warehouse.UnitTests.Mapping
{
    public class OrderMappingTest : IClassFixture<MapperFixture>
    {
        private readonly IMapper _mapper;

        public OrderMappingTest(MapperFixture mapperFixture)
        {
            _mapper = mapperFixture.Mapper;
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledCreateOrderRequestAllValuesShouldBeMappedCorrectly(CreateOrderRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Order>(request);

            // assert
            actual.ProductId.Should().Be(request.ProductId);
            actual.Quantity.Should().Be(request.Quantity);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledOrderDTOAllValuesShouldBeMappedCorrectly(Order request)
        {
            // arrange
            // act
            var actual = _mapper.Map<OrderDTO>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.ProductId.Should().Be(request.ProductId);
            actual.Quantity.Should().Be(request.Quantity);
            actual.Status.Should().Be(request.Status);
        }
    }
}

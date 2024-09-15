using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Models.Product;

namespace Warehouse.UnitTests.Mapping
{
    public class ProductMappingTest : IClassFixture<MapperFixture>
    {
        private readonly IMapper _mapper;

        public ProductMappingTest(MapperFixture mapperFixture)
        {
            _mapper = mapperFixture.Mapper;
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledCreateProductRequestAllValuesShouldBeMappedCorrectly(CreateProductRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Product>(request);

            // assert
            actual.Name.Should().Be(request.Name);
            actual.Stock.Should().Be(request.Stock);
            actual.CategoryId.Should().Be(request.CategoryId);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledUpdateProductRequestAllValuesShouldBeMappedCorrectly(UpdateProductRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Product>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.Name.Should().Be(request.Name);
            actual.CategoryId.Should().Be(request.CategoryId);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledUpdateProductStockRequestAllValuesShouldBeMappedCorrectly(UpdateProductStockRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Product>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.Stock.Should().Be(request.Stock);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledProductDTOAllValuesShouldBeMappedCorrectly(Product request)
        {
            // arrange
            // act
            var actual = _mapper.Map<ProductDTO>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.Name.Should().Be(request.Name);
            actual.CategoryId.Should().Be(request.CategoryId);
            actual.Stock.Should().Be(request.Stock);
        }
    }
}

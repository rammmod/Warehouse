using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Models.Category;

namespace Warehouse.UnitTests.Mapping
{
    public class CategoryMappingTest : IClassFixture<MapperFixture>
    {
        private readonly IMapper _mapper;

        public CategoryMappingTest(MapperFixture mapperFixture)
        {
            _mapper = mapperFixture.Mapper;
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledCreateCategoryRequestAllValuesShouldBeMappedCorrectly(CreateCategoryRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Category>(request);

            // assert
            actual.Name.Should().Be(request.Name);
            actual.OutOfStock.Should().Be(request.OutOfStock);
            actual.LowStock.Should().Be(request.LowStock);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledUpdateCategoryRequestAllValuesShouldBeMappedCorrectly(UpdateCategoryRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Category>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.Name.Should().Be(request.Name);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledUpdateCategoryStockRequestAllValuesShouldBeMappedCorrectly(UpdateCategoryStockRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Category>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.OutOfStock.Should().Be(request.OutOfStock);
            actual.LowStock.Should().Be(request.LowStock);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledDeleteCategoryStockRequestAllValuesShouldBeMappedCorrectly(DeleteCategoryRequest request)
        {
            // arrange
            // act
            var actual = _mapper.Map<Category>(request);

            // assert
            actual.Id.Should().Be(request.Id);
        }

        [Theory]
        [AutoData]
        public void WhenMapperCalledCategoryDTOAllValuesShouldBeMappedCorrectly(Category request)
        {
            // arrange
            // act
            var actual = _mapper.Map<CategoryDTO>(request);

            // assert
            actual.Id.Should().Be(request.Id);
            actual.Name.Should().Be(request.Name);
            actual.OutOfStock.Should().Be(request.OutOfStock);
            actual.LowStock.Should().Be(request.LowStock);
        }
    }
}

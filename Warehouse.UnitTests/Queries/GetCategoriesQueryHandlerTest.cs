using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Queries.Category;
using Moq;

namespace Warehouse.UnitTests.Queries
{
    public class GetCategoriesQueryHandlerTest
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnCategories(Mock<ICategoryRepository> categoryRepository, Mock<IMapper> mapper)
        {
            var categories = new List<Category>() {
                new Category()
                {
                    Id = 1,
                    Name = "Category",
                    OutOfStock = 10,
                    LowStock = 5
                }
            };

            var categoriesDTO = new List<CategoryDTO>() {
                new CategoryDTO()
                {
                    Id = categories.First().Id,
                    Name = categories.First().Name,
                    OutOfStock = categories.First().OutOfStock,
                    LowStock = categories.First().LowStock
                }
            };

            mapper.Setup(x => x.Map<IList<CategoryDTO>>(categories)).Returns(categoriesDTO);

            categoryRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(categories);

            var query = new GetCategoriesQuery();

            var queryHandler = new GetCategoriesQueryHandler(categoryRepository.Object, mapper.Object);

            var response = await queryHandler.Handle(query, CancellationToken.None);

            response.Should().NotBeNull();
            response.Count.Should().Be(1);
            response.Should().BeEquivalentTo(categoriesDTO);
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnEmptyCategoriesWhenCategoriesDoesNotExist(Mock<ICategoryRepository> categoryRepository, Mock<IMapper> mapper)
        {
            mapper.Setup(x => x.Map<IList<CategoryDTO>>(null)).Returns(new List<CategoryDTO>());

            categoryRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync((IList<Category>)null);

            var query = new GetCategoriesQuery();

            var queryHandler = new GetCategoriesQueryHandler(categoryRepository.Object, mapper.Object);

            var response = await queryHandler.Handle(query, CancellationToken.None);

            response.Should().NotBeNull();
            response.Count.Should().Be(0);
        }

        private class AutoMoqDataAttribute : AutoDataAttribute
        {
            public AutoMoqDataAttribute()
                : base(() => new Fixture().Customize(new AutoMoqCustomization()))
            {
            }
        }
    }
}

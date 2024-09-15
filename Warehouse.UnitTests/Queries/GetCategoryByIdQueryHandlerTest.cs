using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.DTOs;
using WarehouseApi.Exceptions;
using WarehouseApi.Queries.Category;
using Moq;

namespace Warehouse.UnitTests.Queries
{
    public class GetCategoryByIdQueryHandlerTest
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnCategoryWhenDataIsValid(Mock<ICategoryRepository> categoryRepository, Mock<IMapper> mapper, Mock<IValidator<int>> validator)
        {
            var category = new Category()
            {
                Id = 1,
                Name = "Category",
                OutOfStock = 10,
                LowStock = 5
            };

            var categoryDTO = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                OutOfStock = category.OutOfStock,
                LowStock = category.LowStock
            };

            mapper.Setup(x => x.Map<CategoryDTO>(category)).Returns(categoryDTO);

            categoryRepository.Setup(x => x.GetAsync(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);

            validator.Setup(x => x.Validate(category.Id));

            var query = new GetCategoryByIdQuery(category.Id);

            var queryHandler = new GetCategoryByIdQueryHandler(validator.Object, categoryRepository.Object, mapper.Object);

            var response = await queryHandler.Handle(query, CancellationToken.None);

            response.Should().NotBeNull();
            response.Id.Should().Be(categoryDTO.Id);
            response.Name.Should().Be(categoryDTO.Name);
            response.LowStock.Should().Be(categoryDTO.LowStock);
            response.OutOfStock.Should().Be(categoryDTO.OutOfStock);
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnError404WhenDataDoesNotExist(Mock<ICategoryRepository> categoryRepository, Mock<IMapper> mapper, Mock<IValidator<int>> validator)
        {
            mapper.Setup(x => x.Map<CategoryDTO>(null)).Returns((CategoryDTO)null);

            categoryRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Category)null);

            int requestId = 1;

            validator.Setup(x => x.Validate(requestId));

            var query = new GetCategoryByIdQuery(requestId);

            var queryHandler = new GetCategoryByIdQueryHandler(validator.Object, categoryRepository.Object, mapper.Object);

            Func<Task> result = async () => await queryHandler.Handle(query, CancellationToken.None);

            await result.Should().ThrowAsync<CategoryNotFoundException>();
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

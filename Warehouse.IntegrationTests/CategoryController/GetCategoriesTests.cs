using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Warehouse.IntegrationTests.Infrastructure;
using WarehouseApi;
using WarehouseApi.DTOs;
using WarehouseApi.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;

namespace Warehouse.IntegrationTests.CategoryController
{
    public class GetCategoriesTests : IClassFixture<IntegrationTestWebAppFactory<Program>>, IClassFixture<AutoDataFixture>
    {
        private readonly WebApplicationFactory<Program> _webFactory;
        private readonly IntegrationTestWebAppFactory<Program> _testFactoryBase;

        public AutoDataFixture DataAutoFixture { get; }

        public ISender Sender = Substitute.For<ISender>();
        public IMapper Mapper { get; set; } = Substitute.For<IMapper>();
        public IFixture AutoFixture => DataAutoFixture.AutoFixture;

        public GetCategoriesTests(IntegrationTestWebAppFactory<Program> program, AutoDataFixture autoDataFixture)
        {

            DataAutoFixture = autoDataFixture;

            _testFactoryBase = program;
            _webFactory = program.WithWebHostBuilder(c =>
            {
                c.ConfigureServices(services =>
                {
                    services.AddScoped(_ => Sender);
                    services.AddScoped(_ => Mapper);
                });
            });
        }

        [Fact]
        public async Task WhenCategoriesExistThenReturnCategory()
        {
            var category = AutoFixture.Create<CategoryDTO>();

            var categoryListItem = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                OutOfStock = category.OutOfStock,
                LowStock = category.LowStock
            };

            var categoryList = new List<CategoryDTO>() { categoryListItem };

            Sender.Send(Arg.Any<GetCategoriesQuery>(), Arg.Any<CancellationToken>()).Returns(categoryList);

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category";

            var response = await client.GetAsync(targetUrl);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = JsonConvert.DeserializeObject<ICollection<CategoryDTO>>(body);

            result.Should().NotBeNullOrEmpty();
            result?.Where(x => x is CategoryDTO).Should().HaveCount(1);
            result.Should().BeEquivalentTo(categoryList, c => c.RespectingRuntimeTypes());
        }

        [Fact]
        public async Task WhenCategoriesDoesNotExistThenReturnEmpty()
        {
            Sender.Send(Arg.Any<GetCategoriesQuery>(), Arg.Any<CancellationToken>()).Returns(new List<CategoryDTO>());

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category";

            var response = await client.GetAsync(targetUrl);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = JsonConvert.DeserializeObject<ICollection<CategoryDTO>>(body);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            result.Should().BeEquivalentTo(new List<CategoryDTO>(), c => c.RespectingRuntimeTypes());
        }
    }
}

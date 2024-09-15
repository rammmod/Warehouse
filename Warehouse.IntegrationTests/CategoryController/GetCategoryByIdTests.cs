using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Warehouse.IntegrationTests.Infrastructure;
using WarehouseApi;
using WarehouseApi.DTOs;
using WarehouseApi.Exceptions;
using WarehouseApi.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;

namespace Warehouse.IntegrationTests.CategoryController
{
    public class GetCategoryByIdTests : IClassFixture<IntegrationTestWebAppFactory<Program>>, IClassFixture<AutoDataFixture>
    {
        private readonly WebApplicationFactory<Program> _webFactory;
        private readonly IntegrationTestWebAppFactory<Program> _testFactoryBase;

        public AutoDataFixture DataAutoFixture { get; }

        public ISender Sender = Substitute.For<ISender>();
        public IMapper Mapper { get; set; } = Substitute.For<IMapper>();
        public IFixture AutoFixture => DataAutoFixture.AutoFixture;

        public GetCategoryByIdTests(IntegrationTestWebAppFactory<Program> program, AutoDataFixture autoDataFixture)
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
        public async Task WhenCategoryExistsThenReturnCategory()
        {
            var category = AutoFixture.Create<CategoryDTO>();

            var categoryModel = new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
                OutOfStock = category.OutOfStock,
                LowStock = category.LowStock
            };

            var query = new GetCategoryByIdQuery(category.Id);

            Sender.Send(query, Arg.Any<CancellationToken>()).Returns(categoryModel);

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category/{categoryModel.Id}";

            var response = await client.GetAsync(targetUrl);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = JsonConvert.DeserializeObject<CategoryDTO>(body);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(categoryModel, c => c.RespectingRuntimeTypes());
        }

        [Fact]
        public async Task WhenCategoryDoesNotExistThenThrowError404()
        {
            var category = AutoFixture.Create<CategoryDTO>();

            Sender.When(x => x.Send(Arg.Any<GetCategoryByIdQuery>(), Arg.Any<CancellationToken>())).Do(x => { throw new CategoryNotFoundException(); });

            await Sender.Send(Arg.Any<GetCategoryByIdQuery>(), Arg.Any<CancellationToken>());

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category/{category.Id}";

            var response = await client.GetAsync(targetUrl);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.NotFound, body);
        }
    }
}

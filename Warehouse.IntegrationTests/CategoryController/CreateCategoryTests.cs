using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Warehouse.IntegrationTests.Infrastructure;
using WarehouseApi;
using WarehouseApi.Commands.Categories;
using WarehouseApi.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace Warehouse.IntegrationTests.CategoryController
{
    public class CreateCategoryTests : IClassFixture<IntegrationTestWebAppFactory<Program>>, IClassFixture<AutoDataFixture>
    {
        private readonly WebApplicationFactory<Program> _webFactory;
        private readonly IntegrationTestWebAppFactory<Program> _testFactoryBase;

        public AutoDataFixture DataAutoFixture { get; }

        public ISender Sender = Substitute.For<ISender>();
        public IMapper Mapper { get; set; } = Substitute.For<IMapper>();
        public IFixture AutoFixture => DataAutoFixture.AutoFixture;

        public CreateCategoryTests(IntegrationTestWebAppFactory<Program> program, AutoDataFixture autoDataFixture)
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
        public async Task WhenRequestReceivedThenSendCommand()
        {
            var command = AutoFixture.Create<CreateCategoryCommand>();

            var categoryDTO = new CategoryDTO()
            {
                Id = 1,
                Name = command.category.Name,
                OutOfStock = command.category.OutOfStock,
                LowStock = command.category.LowStock
            };

            Sender.Send(Arg.Any<CreateCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(categoryDTO.Id);

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category";

            var response = await client.PostAsJsonAsync(targetUrl, command.category);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = Convert.ToInt32(body);

            result.Should().Be(categoryDTO.Id);
        }
    }
}

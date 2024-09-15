using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Warehouse.IntegrationTests.Infrastructure;
using WarehouseApi;
using WarehouseApi.Commands.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace Warehouse.IntegrationTests.CategoryController
{
    public class UpdateCategoryTests : IClassFixture<IntegrationTestWebAppFactory<Program>>, IClassFixture<AutoDataFixture>
    {
        private readonly WebApplicationFactory<Program> _webFactory;
        private readonly IntegrationTestWebAppFactory<Program> _testFactoryBase;

        public AutoDataFixture DataAutoFixture { get; }

        public ISender Sender = Substitute.For<ISender>();
        public IMapper Mapper { get; set; } = Substitute.For<IMapper>();
        public IFixture AutoFixture => DataAutoFixture.AutoFixture;

        public UpdateCategoryTests(IntegrationTestWebAppFactory<Program> program, AutoDataFixture autoDataFixture)
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
        public async Task WhenRequestReceivedThenReturnTrue()
        {
            var command = AutoFixture.Create<UpdateCategoryCommand>();

            Sender.Send(Arg.Any<UpdateCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(true);

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category";

            var response = await client.PutAsJsonAsync(targetUrl, command.category);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = Convert.ToBoolean(body);

            result.Should().Be(true);
        }

        [Fact]
        public async Task WhenRequestReceivedThenReturnFalse()
        {
            var command = AutoFixture.Create<UpdateCategoryCommand>();

            Sender.Send(Arg.Any<UpdateCategoryCommand>(), Arg.Any<CancellationToken>()).Returns(false);

            var client = _webFactory.CreateClient();

            var targetUrl = $"/api/category";

            var response = await client.PutAsJsonAsync(targetUrl, command.category);

            var body = await response.Content.ReadAsStringAsync();

            response.Should().HaveStatusCode(HttpStatusCode.OK, body);

            var result = Convert.ToBoolean(body);

            result.Should().Be(false);
        }
    }
}

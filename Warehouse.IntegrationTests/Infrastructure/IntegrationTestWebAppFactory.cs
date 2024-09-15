using FluentValidation;
using WarehouseApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class IntegrationTestWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
    }
}

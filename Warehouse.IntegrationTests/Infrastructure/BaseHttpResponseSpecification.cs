using Autofac.Extras.Moq;
using Warehouse.Infrastructure;
using WarehouseApi;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Spec;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public abstract class BaseHttpResponseSpecification : ResultSpec<HttpClient, HttpResponseMessage>, IDisposable
    {
        public IntegrationTestWebAppFactory<Program> WebAppFactory { get; }

        protected HttpRequestMessage _httpRequest;
        protected HttpClient _httpClient { get; private set; }

        protected BaseHttpResponseSpecification(IntegrationTestWebAppFactory<Program> webAppFactory)
        {
            WebAppFactory = webAppFactory;
        }

        public virtual void Dispose()
        {
        }
    }
}

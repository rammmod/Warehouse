using WarehouseApi.Contracts;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using MassTransit;

namespace WarehouseApi.Services
{
    public class StockService : IStockService
    {
        readonly ICategoryRepository _categoryRepository;
        readonly IProductRepository _productRepository;
        readonly IPublishEndpoint _publishEndpoint;

        public StockService(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IPublishEndpoint publishEndpoint)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task CheckForStockChange(Product product, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(product.CategoryId, cancellationToken);

            if (category.OutOfStock < product.Stock)
            {
                var productStockChangedContract = new ProductStockChangedContract()
                {
                    CorrelationId = Guid.NewGuid(),
                    ProductId = product.Id
                };

                await _publishEndpoint.Publish(productStockChangedContract, cancellationToken);
            }
        }

        public async Task<bool> UpdateStockAndCheckForChange(Product product, CancellationToken cancellationToken)
        {
            var isStockUpdated = await _productRepository.UpdateStockAsync(product, cancellationToken);

            if (isStockUpdated)
                await CheckForStockChange(product, cancellationToken);

            return isStockUpdated;
        }
    }
}

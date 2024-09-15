using WarehouseApi.Domain.Entities;

namespace WarehouseApi.Services
{
    public interface IStockService
    {
        public Task CheckForStockChange(Product product, CancellationToken cancellationToken);

        public Task<bool> UpdateStockAndCheckForChange(Product product, CancellationToken cancellationToken);
    }
}

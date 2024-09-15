using WarehouseApi.Domain.Entities;

namespace WarehouseApi.Domain.Abstractions
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAllAsync(CancellationToken cancellationToken);
        Task<Product> GetAsync(int Id, CancellationToken cancellationToken);
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateStockAsync(Product product, CancellationToken cancellationToken);
        Task<bool> ExistsInCategory(int categoryId, CancellationToken cancellationToken);
    }
}

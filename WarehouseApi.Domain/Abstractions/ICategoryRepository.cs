using WarehouseApi.Domain.Entities;

namespace WarehouseApi.Domain.Abstractions
{
    public interface ICategoryRepository
    {
        Task<IList<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task<Category> GetAsync(int Id, CancellationToken cancellationToken);
        Task<Category> AddAsync(Category category, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<bool> UpdateStockAsync(Category category, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken);
    }
}

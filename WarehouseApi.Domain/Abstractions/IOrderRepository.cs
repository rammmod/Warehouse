using WarehouseApi.Domain.Entities;

namespace WarehouseApi.Domain.Abstractions
{
    public interface IOrderRepository
    {
        Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken);
        Task<Order> GetAsync(int Id, CancellationToken cancellationToken);
        Task<Order> AddAsync(Order order, CancellationToken cancellationToken);
        Task<bool> ProceedAsync(Order order, CancellationToken cancellationToken);
    }
}

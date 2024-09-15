using Warehouse.Infrastructure.Exceptions;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using MongoDB.Driver;

namespace Warehouse.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public async Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Collection.Find(FilterDefinition<Order>.Empty).ToListAsync(cancellationToken);
        }

        public async Task<Order> GetAsync(int Id, CancellationToken cancellationToken)
        {
            return await Collection.Find(m => m.Id == Id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
        {
            order.Id = GenerateNextId(nameof(Order));

            await Collection.InsertOneAsync(order, null, cancellationToken);

            return order;
        }

        public async Task<bool> ProceedAsync(Order order, CancellationToken cancellationToken)
        {
            var update = Builders<Order>.Update
                .Set(x => x.Status, order.Status);

            var updated = await Collection.UpdateOneAsync(m => m.Id == order.Id, update, null, cancellationToken);

            if (!updated.IsAcknowledged)
                throw new UpdateNotAcknowledgedException();

            return updated.ModifiedCount == 1;
        }
    }
}

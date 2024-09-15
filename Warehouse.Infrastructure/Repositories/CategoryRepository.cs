using Warehouse.Infrastructure.Exceptions;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using MongoDB.Driver;

namespace Warehouse.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public async Task<IList<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Collection.Find(FilterDefinition<Category>.Empty).ToListAsync(cancellationToken);
        }

        public async Task<Category> GetAsync(int Id, CancellationToken cancellationToken)
        {
            return await Collection.Find(m => m.Id == Id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken)
        {
            category.Id = GenerateNextId(nameof(Category));

            await Collection.InsertOneAsync(category, null, cancellationToken);

            return category;
        }

        public async Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            var update = Builders<Category>.Update
                .Set(x => x.Name, category.Name);

            var updated = await Collection.UpdateOneAsync(m => m.Id == category.Id, update, null, cancellationToken);

            if (!updated.IsAcknowledged)
                throw new UpdateNotAcknowledgedException();

            return updated.ModifiedCount == 1;
        }

        public async Task<bool> UpdateStockAsync(Category category, CancellationToken cancellationToken)
        {
            var update = Builders<Category>.Update
                .Set(x => x.OutOfStock, category.OutOfStock)
                .Set(x => x.LowStock, category.LowStock);

            var updated = await Collection.UpdateOneAsync(m => m.Id == category.Id, update, null, cancellationToken);

            if (!updated.IsAcknowledged)
                throw new UpdateNotAcknowledgedException();

            return updated.ModifiedCount == 1;
        }

        public async Task<bool> DeleteAsync(int Id, CancellationToken cancellationToken)
        {
            var deleted = await Collection.DeleteOneAsync(m => m.Id == Id, null, cancellationToken);

            if (!deleted.IsAcknowledged)
                throw new DeleteNotAcknowledgedException();

            return deleted.DeletedCount == 1;
        }
    }
}

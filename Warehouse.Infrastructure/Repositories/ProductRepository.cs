using Warehouse.Infrastructure.Exceptions;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using MongoDB.Driver;

namespace Warehouse.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
        }

        public async Task<IList<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Collection.Find(FilterDefinition<Product>.Empty).ToListAsync(cancellationToken);
        }

        public async Task<Product> GetAsync(int Id, CancellationToken cancellationToken)
        {
            return await Collection.Find(m => m.Id == Id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
        {
            product.Id = GenerateNextId(nameof(Product));

            await Collection.InsertOneAsync(product, null, cancellationToken);

            return product;
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            var update = Builders<Product>.Update
                .Set(x => x.Name, product.Name)
                .Set(x => x.CategoryId, product.CategoryId);

            var updated = await Collection.UpdateOneAsync(m => m.Id == product.Id, update, null, cancellationToken);

            if (!updated.IsAcknowledged)
                throw new UpdateNotAcknowledgedException();

            return updated.ModifiedCount == 1;
        }

        public async Task<bool> UpdateStockAsync(Product product, CancellationToken cancellationToken)
        {
            var update = Builders<Product>.Update
                .Set(x => x.Stock, product.Stock);

            var updated = await Collection.UpdateOneAsync(m => m.Id == product.Id, update, null, cancellationToken);

            if (!updated.IsAcknowledged)
                throw new UpdateNotAcknowledgedException();

            return updated.ModifiedCount == 1;
        }

        public async Task<bool> ExistsInCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await Collection.Find(m => m.CategoryId == categoryId, null).AnyAsync(cancellationToken);
        }
    }
}

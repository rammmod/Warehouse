using MongoDB.Driver;

namespace Warehouse.Infrastructure.Repositories
{
    public class BaseRepository<T>
    {
        readonly IMongoDbContext _mongoDbContext;

        public BaseRepository(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        protected IMongoCollection<T> Collection => _mongoDbContext.GetCollection<T>();
        protected int GenerateNextId(string collectionName) => _mongoDbContext.GenerateNextId(collectionName);
    }
}
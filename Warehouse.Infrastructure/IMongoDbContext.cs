using MongoDB.Driver;

namespace Warehouse.Infrastructure
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>();
        int GenerateNextId(string collectionName);
    }
}

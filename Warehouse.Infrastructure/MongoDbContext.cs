using MongoDB.Bson;
using MongoDB.Driver;

namespace Warehouse.Infrastructure
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly MongoDbConfiguration _mongoDbConfiguration;
        private readonly IMongoCollection<BsonDocument> _countersCollection;

        public MongoDbContext(IMongoClient mongoClient, MongoDbConfiguration mongoDbConfiguration)
        {
            _mongoClient = mongoClient;
            _mongoDbConfiguration = mongoDbConfiguration;
            _countersCollection = _mongoClient.GetDatabase(_mongoDbConfiguration.DatabaseName, new MongoDatabaseSettings()).GetCollection<BsonDocument>("counters");
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var db = _mongoClient.GetDatabase(_mongoDbConfiguration.DatabaseName, new MongoDatabaseSettings());
            return db.GetCollection<T>(typeof(T).Name);
        }

        public int GenerateNextId(string collectionName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", collectionName);
            var update = Builders<BsonDocument>.Update.Inc("seq", 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = _countersCollection.FindOneAndUpdate(filter, update, options);
            return result["seq"].AsInt32;
        }
    }
}

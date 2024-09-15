using MongoDB.Driver;

namespace Warehouse.Infrastructure
{
    public class MongoDbClientFactory
    {
        private readonly MongoDbConfiguration _configuration;

        public MongoDbClientFactory(MongoDbConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMongoClient Get()
        {
            var clientSettings = MongoClientSettings.FromConnectionString(_configuration.ConnectionString);
            return new MongoClient(clientSettings);
        }
    }
}

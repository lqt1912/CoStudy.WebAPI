using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CoStudy.API.Infrastructure.Persistence.Contexts
{
    public class CustomMongoClient
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        IConfiguration configuration;
        public CustomMongoClient(IConfiguration config)
        {
            configuration = config;
           _client = new MongoClient(configuration["MongoConnectionString"]);
            _database = _client.GetDatabase("CoStudyServerDb");
        }

        public MongoClient GetClient()
        {
            return _client;
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CoStudy.API.Infrastructure.Identity.Contexts
{
    public class IdentityMongoClient
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private IConfiguration configuration;
        public IdentityMongoClient(IConfiguration configuration)
        {
            this.configuration = configuration;
            _client = new MongoClient(configuration["MongoConnectionString"]);
            _database = _client.GetDatabase("Default");

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

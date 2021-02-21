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
           // _client = new MongoClient("mongodb+srv://admin:lqt%401912@cluster0.qxh5d.azure.mongodb.net/test?authSource=admin&replicaSet=atlas-2nteux-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
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

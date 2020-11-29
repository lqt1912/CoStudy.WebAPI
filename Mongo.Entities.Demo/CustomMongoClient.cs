using MongoDB.Driver;

namespace Mongo.Entities.Demo
{
    public class CustomMongoClient
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        public CustomMongoClient()
        {
            _client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            _database = _client.GetDatabase("bookshop");
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

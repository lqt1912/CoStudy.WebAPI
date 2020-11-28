using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Persistence.Contexts
{
    public class CustomMongoClient
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        public CustomMongoClient()
        {
            _client = new MongoClient("mongodb+srv://adminlqt%401912@cluster0.qxh5d.azure.mongodb.net/test?authSource=admin&replicaSet=atlas-2nteux-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true");
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

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CoStudy.API.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// Class CustomMongoClient
    /// </summary>
    public class CustomMongoClient
    {
        /// <summary>
        /// The client
        /// </summary>
        private MongoClient _client;
        /// <summary>
        /// The database
        /// </summary>
        private IMongoDatabase _database;
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMongoClient"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public CustomMongoClient(IConfiguration config)
        {
            configuration = config;
           _client = new MongoClient(configuration["MongoConnectionString"]);
            _database = _client.GetDatabase("CoStudyServerDb");
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns></returns>
        public MongoClient GetClient()
        {
            return _client;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}

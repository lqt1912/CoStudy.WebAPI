using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CoStudy.API.Infrastructure.Identity.Contexts
{
    /// <summary>
    /// class IdentityMongoClient
    /// </summary>
    public class IdentityMongoClient
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
        private IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityMongoClient"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public IdentityMongoClient(IConfiguration configuration)
        {
            this.configuration = configuration;
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

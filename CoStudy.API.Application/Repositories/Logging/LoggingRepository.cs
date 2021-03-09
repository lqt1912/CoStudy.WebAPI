using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class LoggingRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Logging}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.ILoggingRepository" />
    public class LoggingRepository : BaseRepository<Logging>, ILoggingRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LoggingRepository(IConfiguration configuration) : base("logging", configuration)
        {
            this.configuration = configuration;
        }
    }
}

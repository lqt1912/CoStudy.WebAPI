using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class LoggingRepository : BaseRepository<Logging>, ILoggingRepository
    {
        IConfiguration configuration;
        public LoggingRepository(IConfiguration configuration) : base("logging", configuration)
        {
            this.configuration = configuration;
        }
    }
}

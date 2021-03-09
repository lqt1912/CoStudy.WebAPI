using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class NofticationRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Noftication}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.INofticationRepository" />
    public class NofticationRepository : BaseRepository<Noftication>, INofticationRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NofticationRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public NofticationRepository(IConfiguration configuration) : base("noftication", configuration)
        {
            this.configuration = configuration;
        }
    }
}

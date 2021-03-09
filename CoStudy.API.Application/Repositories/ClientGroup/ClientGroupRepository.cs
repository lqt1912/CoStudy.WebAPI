using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class ClientGroupRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.ClientGroup}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IClientGroupRepository" />
    public class ClientGroupRepository : BaseRepository<ClientGroup>, IClientGroupRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGroupRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ClientGroupRepository(IConfiguration configuration) : base("client_group", configuration)
        {
            this.configuration = configuration;
        }
    }
}

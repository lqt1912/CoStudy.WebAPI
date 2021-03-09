using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class UserRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.User}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IUserRepository" />
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public UserRepository(IConfiguration configuration) : base("user", configuration)
        {
            this.configuration = configuration;
        }
    }
}

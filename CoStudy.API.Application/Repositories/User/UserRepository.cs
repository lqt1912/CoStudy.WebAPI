using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        IConfiguration configuration;
        public UserRepository(IConfiguration configuration) : base("user", configuration)
        {
            this.configuration = configuration;
        }
    }
}

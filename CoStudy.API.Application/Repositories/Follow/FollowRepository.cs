using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class FollowRepository : BaseRepository<Follow>, IFollowRepository
    {
        IConfiguration configuration;
        public FollowRepository(IConfiguration configuration) : base("follow", configuration)
        {
            this.configuration = configuration;
        }

    }
}

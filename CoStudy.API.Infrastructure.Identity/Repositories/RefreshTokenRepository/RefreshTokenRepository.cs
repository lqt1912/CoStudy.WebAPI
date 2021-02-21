using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Infrastructure.Identity.Repositories.RefreshTokenRepository
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        IConfiguration configuration;
        public RefreshTokenRepository(IConfiguration configuration) : base("refresh_token", configuration)
        {
            this.configuration = configuration;
        }
    }
}

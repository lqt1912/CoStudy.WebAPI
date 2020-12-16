using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Infrastructure.Identity.Repositories.RefreshTokenRepository
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository() : base("refresh_token")
        {

        }
    }
}

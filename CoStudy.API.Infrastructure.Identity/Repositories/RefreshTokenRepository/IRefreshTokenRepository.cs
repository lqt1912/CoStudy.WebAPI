using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Infrastructure.Identity.Repositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
    }
}

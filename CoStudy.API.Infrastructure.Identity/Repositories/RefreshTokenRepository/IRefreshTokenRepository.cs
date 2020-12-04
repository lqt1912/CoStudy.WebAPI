using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Repositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
    }
}

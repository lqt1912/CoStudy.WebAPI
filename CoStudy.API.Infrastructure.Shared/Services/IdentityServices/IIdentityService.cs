using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface IIdentityService
    {
             IEnumerable<User> GetAllUser(BaseGetAllRequest request);
             IEnumerable<Account> GetAllAccount(BaseGetAllRequest request);
             Task<IEnumerable<RefreshToken>> GetByAccount(string accountId);

    }
}

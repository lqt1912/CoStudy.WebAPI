using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
        public class IdentityService : IIdentityService
    {
           IUserRepository userRepository;

           IAccountRepository accountRepository;

              public IdentityService(IUserRepository userService, IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
        }

             public IEnumerable<User> GetAllUser(BaseGetAllRequest request)
        {
            var data = userRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return data;
        }

             public IEnumerable<Account> GetAllAccount(BaseGetAllRequest request)
        {
            var data = accountRepository.GetAll();
            if (request.Skip.HasValue & request.Count.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }

              public async Task<IEnumerable<RefreshToken>> GetByAccount(string accountId)
        {
            var account = await accountRepository.GetByIdAsync(ObjectId.Parse(accountId));
            if (account != null)
            {
                return account.RefreshTokens;
            }
            throw new Exception("KHông tìm thấy tài khoản. ");
        }
    }
}

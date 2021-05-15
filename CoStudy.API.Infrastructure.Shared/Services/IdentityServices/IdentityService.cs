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
    /// <summary>
    /// The identity  service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IIdentityService" />
    public class IdentityService : IIdentityService
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        IAccountRepository accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public IdentityService(IUserRepository userService, IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Gets all user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUser(BaseGetAllRequest request)
        {
            var data = userRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return data;
        }

        /// <summary>
        /// Gets all account.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Account> GetAllAccount(BaseGetAllRequest request)
        {
            var data = accountRepository.GetAll();
            if (request.Skip.HasValue & request.Count.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }

        /// <summary>
        /// Gets the by account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">KHông tìm thấy tài khoản.</exception>
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

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Identity Service interface
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Gets all user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<User> GetAllUser(BaseGetAllRequest request);
        /// <summary>
        /// Gets all account.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<Account> GetAllAccount(BaseGetAllRequest request);
        /// <summary>
        /// Gets the by account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<RefreshToken>> GetByAccount(string accountId);

    }
}

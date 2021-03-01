using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Level Service Interface
    /// </summary>
    public interface ILevelService
    {
        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        Task<IEnumerable<Level>> AddLevel(IEnumerable<Level> level);
        /// <summary>
        /// Gets all level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<Level> GetAllLevel(BaseGetAllRequest request);
        /// <summary>
        /// Bets the gy identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Level> BetGyId(string id);

        /// <summary>
        /// Adds the object level.
        /// </summary>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns></returns>
        Task<IEnumerable<ObjectLevel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels);

        /// <summary>
        /// Gets the level by object.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ObjectLevel>> GetLevelByObject(string objectId);



        /// <summary>
        /// Adds the fields for user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<User> AddFieldsForUser(UserAddFieldRequest request);

        /// <summary>
        /// Gets the fields of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<UserGetFieldResponse>> GetFieldsOfUser(string userId);
    }
}

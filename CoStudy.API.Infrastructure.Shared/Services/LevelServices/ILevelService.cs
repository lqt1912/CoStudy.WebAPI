using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
using CoStudy.API.Infrastructure.Shared.ViewModels;
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
        /// Adds the or update user fields.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> AddOrUpdateUserFields(UserAddFieldRequest request);

        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        Task<IEnumerable<LevelViewModel>> AddLevel(IEnumerable<Level> level);

        /// <summary>
        /// Adds the object level.
        /// </summary>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns></returns>
        Task<IEnumerable<ObjectLevelViewModel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<LevelViewModel> GetById(string id);

        /// <summary>
        /// Gets all level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<LevelViewModel> GetAllLevel(BaseGetAllRequest request);
        /// <summary>
        /// Gets the fields of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ObjectLevelViewModel>> GetFieldsOfUser(string userId);

        /// <summary>
        /// Gets the level by object.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<ObjectLevelViewModel>> GetLevelByObject(string objectId);

        /// <summary>
        /// Determines whether the specified user object levels is match.
        /// </summary>
        /// <param name="userObjectLevels">The user object levels.</param>
        /// <param name="postObjectLevels">The post object levels.</param>
        /// <returns>
        ///   <c>true</c> if the specified user object levels is match; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(IEnumerable<ObjectLevelViewModel> userObjectLevels, IEnumerable<ObjectLevelViewModel> postObjectLevels);

        /// <summary>
        /// Gets all post match user.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllPostMatchUser(string postId);

        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ObjectLevelViewModel> AddPoint(AddPointRequest request);

        /// <summary>
        /// Resets the field.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> ResetField(UserResetFieldRequest request);

        /// <summary>
        /// Updates the post field.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<PostViewModel> UpdatePostField(UpdatePostLevelRequest request);


    }
}

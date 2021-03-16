using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    /// <summary>
    /// The User Service Interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        Task<Field> AddField(string fieldValue);
        /// <summary>
        /// Adds the user asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<UserViewModel> AddUserAsync(AddUserRequest entity);

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> UpdateUserAsync(UpdateUserRequest request);

        /// <summary>
        /// Adds the avatar asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> AddAvatarAsync(AddAvatarRequest request);
        /// <summary>
        /// Updates the avatar asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> UpdateAvatarAsync(AddAvatarRequest request);


        /// <summary>
        /// Adds the followings asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<string> AddFollowingsAsync(AddFollowerRequest request);


        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<UserViewModel> GetUserById(string id);
      

        /// <summary>
        /// Removes the following.
        /// </summary>
        /// <param name="followerId">The follower identifier.</param>
        /// <returns></returns>
        Task<string> RemoveFollowing(string followerId);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        List<Field> GetAll();

        /// <summary>
        /// Gets the follower.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<FollowViewModel>> GetFollower(FollowFilterRequest request);

        /// <summary>
        /// Gets the following.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<FollowViewModel>> GetFollowing(FollowFilterRequest request);

        /// <summary>
        /// Filters the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<UserViewModel>> FilterUser(FilterUserRequest request);

        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<UserViewModel> AddInfo(List<IDictionary<string, string>> request);

        UserViewModel GetCurrentUser();
    }
}

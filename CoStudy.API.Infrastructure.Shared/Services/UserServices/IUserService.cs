using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public interface IUserService
    {
        Task<Field> AddField(string fieldValue);
        Task<AddUserResponse> AddUserAsync(AddUserRequest entity);

        Task<User> UpdateUserAsync(UpdateUserRequest request);

        Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request);
        Task<User> UpdateAvatarAsync(AddAvatarRequest request);

        Task<AddFollowerResponse> AddFollowersAsync(AddFollowerRequest request);

        Task<AddFollowerResponse> AddFollowingsAsync(AddFollowerRequest request);

        Task<AddAdditionalInfoResponse> AddAdditonalInfoAsync(AddAdditionalInfoRequest request);

        Task<User> AddFieldAsync(AddFieldRequest request);

        Task<GetUserByIdResponse> GetUserById(string id);
        Task SyncPost();
        GetUserByIdResponse GetCurrentUser();

        Task<User> RemoveFollowing(string followerId);

        List<Field> GetAll();
    }
}

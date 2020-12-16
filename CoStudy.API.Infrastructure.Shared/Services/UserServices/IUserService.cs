using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public interface IUserService
    {
        Task<AddUserResponse> AddUserAsync(AddUserRequest entity);

        Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request);

        Task<AddFollowerResponse> AddFollowersAsync(AddFollowerRequest request);

        Task<AddFollowerResponse> AddFollowingsAsync(AddFollowerRequest request);

        Task<AddAdditionalInfoResponse> AddAdditonalInfoAsync(AddAdditionalInfoRequest request);

        Task<AddFieldResponse> AddFieldAsync(AddFieldRequest request);

        Task<GetUserByIdResponse> GetUserById(string id);
        Task SyncPost();
        GetUserByIdResponse GetCurrentUser();
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
       public interface IUserService
    {
             Task<Field> AddField(string fieldValue);
             Task<UserViewModel> AddUserAsync(AddUserRequest entity);

             Task<UserViewModel> UpdateUserAsync(UpdateUserRequest request);

             Task<UserViewModel> UpdateUserAddress(Address request);
             Task<UserViewModel> AddAvatarAsync(AddAvatarRequest request);
             Task<UserViewModel> UpdateAvatarAsync(AddAvatarRequest request);


             Task<string> AddFollowingsAsync(AddFollowerRequest request);


             Task<UserViewModel> GetUserById(string id);


             Task<string> RemoveFollowing(string followerId);

            List<Field> GetAll();

             Task<IEnumerable<FollowViewModel>> GetFollower(FollowFilterRequest request);

             Task<IEnumerable<FollowViewModel>> GetFollowing(FollowFilterRequest request);

             Task<IEnumerable<UserViewModel>> FilterUser(FilterUserRequest request);

             Task<UserViewModel> AddOrUpdateInfo(List<AdditionalInfomation> request);

            UserViewModel GetCurrentUser();

            IEnumerable<UserNearbyViewModel> GetNearbyUser(BaseGetAllRequest baseGetAllRequest);

             Task<UserViewModel> AddOrUpdateCallId(AddOrUpdateCallIdRequest request);
             Task<bool> IsEmailExist(string email);
             Task<UserViewModel> ModifiedUser(ModifiedRequest request);
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public interface IUserService
    {
        User GetByEmail(string email);

        Task UpdateAsyc(User entity, string Id);
        Task<AddUserResponse> AddUserAsync(AddUserRequest entity);

        Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request);

        Task<AddFollowerResponse> AddFollowersAsync(AddFollowerRequest request);

        Task<AddFollowerResponse> AddFollowingsAsync(AddFollowerRequest request);

        Task<AddAdditionalInfoResponse> AddAdditonalInfoAsync(AddAdditionalInfoRequest request);

        Task<AddFieldResponse> AddFieldAsync(AddFieldRequest request);


    }
}

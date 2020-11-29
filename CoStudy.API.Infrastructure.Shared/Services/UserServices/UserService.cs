using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public class UserService:IUserService
    {
        IUserRepository userRepository;
        IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<AddAdditionalInfoResponse> AddAdditonalInfoAsync(AddAdditionalInfoRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request,_httpContextAccessor);

            var currentUser = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            currentUser.Avatar = avatar;

            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, ObjectId.Parse(request.UserId));

            return UserAdapter.ToResponse(avatar,request.UserId);

        }

        public async Task<AddFollowerResponse> AddFollowersAsync(AddFollowerRequest request)
        {
            var currentUser = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            currentUser.Followers.AddRange(request.Followers);
            currentUser.Followers = currentUser.Followers.Distinct().ToList();
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, ObjectId.Parse(request.UserId));

            return new AddFollowerResponse()
            {
                UserId = request.UserId,
                Followers = request.Followers.Distinct().ToList()
            };
        }

        /// <summary>
        /// Copy xuống thôi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AddFollowerResponse> AddFollowingsAsync(AddFollowerRequest request)
        {
            var currentUser = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            currentUser.Following.AddRange(request.Followers);
            currentUser.Following = currentUser.Following.Distinct().ToList();
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, ObjectId.Parse(request.UserId));

            return new AddFollowerResponse()
            {
                UserId = request.UserId,
                Followers = request.Followers.Distinct().ToList()
            };
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest entity)
        {
            var user = UserAdapter.FromRequest(entity);

            await userRepository.AddAsync(user);

            return UserAdapter.ToResponse(user);
        }

    }
}

using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        IAccountRepository accountRepository;
        IHttpContextAccessor _httpContextAccessor;
        IConfiguration _configuration;

        public UserService(IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration, 
            IAccountRepository accountRepository)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            this.accountRepository = accountRepository;
        }

        public async Task<AddAdditionalInfoResponse> AddAdditonalInfoAsync(AddAdditionalInfoRequest request)
        {
            var additionalInfos = UserAdapter.FromRequest(request);

            var currentUser = CurrentUser();

            currentUser.AdditionalInfos.AddRange(additionalInfos);
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return UserAdapter.ToResponse(additionalInfos, currentUser.Id.ToString());
        }

        public async Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request,_httpContextAccessor);

            var currentUser = CurrentUser();

            currentUser.Avatar = avatar;

            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return UserAdapter.ToResponse(avatar,currentUser.Id.ToString());

        }

        public async Task<AddFieldResponse> AddFieldAsync(AddFieldRequest request)
        {
            var field = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = CurrentUser();
            currentUser.Fortes.Add(field);
            currentUser.ModifiedDate = DateTime.Now;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);
            return UserAdapter.ToResponse(field, currentUser.Id.ToString());
        }

        public async Task<AddFollowerResponse> AddFollowersAsync(AddFollowerRequest request)
        {
            var currentUser = CurrentUser();

            currentUser.Followers.AddRange(request.Followers);
            currentUser.Followers = currentUser.Followers.Distinct().ToList();
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return new AddFollowerResponse()
            {
                UserId = currentUser.Id.ToString(),
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
            var currentUser = CurrentUser();

            currentUser.Following.AddRange(request.Followers);
            currentUser.Following = currentUser.Following.Distinct().ToList();
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return new AddFollowerResponse()
            {
                UserId = currentUser.Id.ToString(),
                Followers = request.Followers.Distinct().ToList()
            };
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest entity)
        {
            var user = UserAdapter.FromRequest(entity);

            await userRepository.AddAsync(user);

            return UserAdapter.ToResponse(user);
        }


        private User FromAccount(Account account)
        {
            var user = userRepository.GetAll().SingleOrDefault(x => x.Email == account.Email);
            return user;
        }

        private User CurrentUser()
        {
            var currentAccount = (Account)_httpContextAccessor.HttpContext.Items["Account"];
            return FromAccount(currentAccount);
        }
    }
}

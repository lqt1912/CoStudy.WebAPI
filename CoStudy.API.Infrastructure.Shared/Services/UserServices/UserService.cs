using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;
        IAccountRepository accountRepository;
        IHttpContextAccessor _httpContextAccessor;
        IConfiguration _configuration;
        IPostRepository postRepository;
        IClientConnectionsRepository clientConnectionsRepository;
        IClientGroupRepository clientGroupRepository;
        public UserService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IAccountRepository accountRepository, IPostRepository postRepository, IClientConnectionsRepository clientConnectionsRepository, IClientGroupRepository clientGroupRepository)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            this.accountRepository = accountRepository;
            this.postRepository = postRepository;
            this.clientConnectionsRepository = clientConnectionsRepository;
            this.clientGroupRepository = clientGroupRepository;
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
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = CurrentUser();

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = avatar.ImageHash;
            currentUser.ModifiedDate = DateTime.Now;

            foreach (var post in postRepository.GetAll().Where(x=>x.AuthorId == currentUser.Id.ToString() ||x.Status ==ItemStatus.Active))
            {
                post.AuthorAvatar = avatar.ImageHash;
                await postRepository.UpdateAsync(post, post.Id);
            }
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return UserAdapter.ToResponse(avatar, currentUser.Id.ToString());

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

            foreach (var item in request.Followers)
            {
                var user = await userRepository.GetByIdAsync(ObjectId.Parse(item));
                if (user != null)
                { 
                    user.Followers.Add(currentUser.Id.ToString());
                    await userRepository.UpdateAsync(user, user.Id);
                }
            }

            return new AddFollowerResponse()
            {
                UserId = currentUser.Id.ToString(),
                Followers = request.Followers.Distinct().ToList()
            };
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest entity)
        {
            var clientConnection = new ClientConnections();

            var user = UserAdapter.FromRequest(entity);
            user.ClientConnectionsId = clientConnection.Id.ToString();

            await userRepository.AddAsync(user);

            await clientConnectionsRepository.AddAsync(clientConnection);

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

        public async Task SyncPost()
        {
            try
            {
                var users = userRepository.GetAll();
                int c = 0;
                foreach (var user in users)
                {

                    var latestUserPosts = postRepository.GetAll().Where(x => x.AuthorId == user.Id.ToString());
                    if (latestUserPosts.Count() > 10)
                        latestUserPosts = latestUserPosts.Take(10);

                    user.Posts.Clear();
                    user.Posts.AddRange(latestUserPosts);
                    user.ModifiedDate = DateTime.Now;
                    await userRepository.UpdateAsync(user, user.Id);
                    c = latestUserPosts.Count();
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        public async Task<GetUserByIdResponse> GetUserById(string id)

        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(id));
            if (user == null)
                throw new Exception("Không tìm thấy user");
            return UserAdapter.ToResponse1(user);
        }

        public GetUserByIdResponse GetCurrentUser()
        {
            try
            {
                var user = Feature.CurrentUser(_httpContextAccessor, userRepository);

                if (user == null)
                    throw new Exception("Không tìm thấy user");
                return UserAdapter.ToResponse1(user);
            }
            catch (Exception)
            {
                throw new Exception("Không tìm thấy user");
            }
        }

        public async Task<User> RemoveFollowing(string followerId)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);

            if (currentUser.Following.Contains(followerId))
            { 
                currentUser.Following.Remove(followerId);
                var user = await userRepository.GetByIdAsync(ObjectId.Parse(followerId));
                if(user!=null)
                {
                    user.Followers.Remove(currentUser.Id.ToString());
                    await userRepository.UpdateAsync(user, user.Id);
                }    
            }
            await userRepository.UpdateAsync(currentUser, currentUser.Id);
            return currentUser;
        }

        public async Task<User> UpdateUserAsync(UpdateUserRequest request)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            currentUser.Address = request.Address;
            currentUser.FirstName = request.FisrtName;
            currentUser.LastName = request.LastName;
            currentUser.PhoneNumber = request.PhoneNumber;
            currentUser.DateOfBirth = request.DateOfBirth;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return currentUser;
        }

        public async Task<User> UpdateAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = CurrentUser();

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = request.AvatarHash;

            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return currentUser;
        }
    }
}

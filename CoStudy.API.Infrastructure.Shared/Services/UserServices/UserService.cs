using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
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
        IClientGroupRepository clientGroupRepository;
        IFieldRepository fieldRepository;
        IFollowRepository followRepository;

        public UserService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            IPostRepository postRepository,
            IClientGroupRepository clientGroupRepository,
            IFieldRepository fieldRepository, IFollowRepository followRepository)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            this.accountRepository = accountRepository;
            this.postRepository = postRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.fieldRepository = fieldRepository;
            this.followRepository = followRepository;
        }


        public async Task<AddAvatarResponse> AddAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = CurrentUser();

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = avatar.ImageHash;
            currentUser.ModifiedDate = DateTime.Now;

            foreach (var post in postRepository.GetAll().Where(x => x.AuthorId == currentUser.Id.ToString() || x.Status == ItemStatus.Active))
            {
                post.AuthorAvatar = avatar.ImageHash;
                await postRepository.UpdateAsync(post, post.Id);
            }
            await userRepository.UpdateAsync(currentUser, currentUser.Id);
            CacheHelper.Delete($"CurrentUser-{currentUser.Email}");
            CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));
            return UserAdapter.ToResponse(avatar, currentUser.Id.ToString());

        }

        

        public async Task<string> AddFollowingsAsync(AddFollowerRequest request)
        {
            var currentUser = CurrentUser();

            foreach (var item in request.Followers)
            {
                var user = await userRepository.GetByIdAsync(ObjectId.Parse(item));
                if (user != null)
                {
                    var filter = Builders<Follow>.Filter;
                    var finder = filter.Eq("from_id", currentUser.OId) & filter.Eq("to_id", item);
                    var existFollowing = await followRepository.FindAsync(finder);
                    if (existFollowing != null)
                    {
                        return "Bạn đã theo dõi người này rồi";
                    }

                    var follow = new Follow()
                    {
                        FromId = currentUser.OId,
                        ToId = item,
                        Avatar = user.Avatar.ImageHash,
                        FullName = $"{user.FirstName} {user.LastName}",
                        FollowDate = DateTime.Now
                    };

                    await followRepository.AddAsync(follow);
                }
            }
            return "Theo dõi thành công";
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest entity)
        {

            var user = UserAdapter.FromRequest(entity);

            await userRepository.AddAsync(user);


            return UserAdapter.ToResponse(user);
        }


        private User FromAccount(Account account)
        {
            //var cacheduser = CacheHelper.GetValue($"CurrentUser-{account.Email}") as User;

            //if (cacheduser != null)
            //    return cacheduser;

            var filter = Builders<User>.Filter.Eq("email", account.Email);
            return userRepository.Find(filter);
        }

        private User CurrentUser()
        {
            var currentAccount = (Account)_httpContextAccessor.HttpContext.Items["Account"];
            return FromAccount(currentAccount);
        }

        public async Task SyncFollow()
        {
            try
            {
                var users = userRepository.GetAll();
                foreach (var user in users)
                {
                    var filter = Builders<Follow>.Filter.Eq("from_id", user.Id.ToString());
                    user.Following = (await followRepository.FindListAsync(filter)).Count();

                    var filter2 = Builders<Follow>.Filter.Eq("to_id", user.Id.ToString());
                    user.Followers = (await followRepository.FindListAsync(filter2)).Count();

                    await userRepository.UpdateAsync(user, user.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        public async Task<User> GetUserById(string id)

        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(id));
            if (user == null)
                throw new Exception("Không tìm thấy user");
            return user;
        }

        public User GetCurrentUser()
        {
            try
            {
                var user = Feature.CurrentUser(_httpContextAccessor, userRepository);

                if (user == null)
                    throw new Exception("Không tìm thấy user");
                return user;
            }
            catch (Exception)
            {
                throw new Exception("Không tìm thấy user");
            }
        }

        public async Task<string> RemoveFollowing(string toFollowerId)
        {
            try
            {
                var findFilter = Builders<Follow>.Filter.Eq("to_id", toFollowerId);
                var following = await followRepository.FindAsync(findFilter);
                await followRepository.DeleteAsync(following.Id);
                return $"Bạn đã bỏ theo dõi người dùng {following.FullName}";
            }
            catch (Exception)
            {
                throw new Exception("Người dùng không tồn tại. Đã có lổi xảy ra");
            }
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

            //CacheHelper.Delete($"CurrentUser-{currentUser.Email}");
            //CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));

            return currentUser;
        }

        public async Task<User> UpdateAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = CurrentUser();

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = request.AvatarHash;

            foreach (var post in postRepository.GetAll()
                .Where(x => x.AuthorId == currentUser.Id.ToString()
                && x.Status == ItemStatus.Active))
            {
                post.AuthorAvatar = avatar.ImageHash;
                await postRepository.UpdateAsync(post, post.Id);
            }

            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            //CacheHelper.Delete($"CurrentUser-{currentUser.Email}");
            //CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));

            return currentUser;
        }

        public async Task<Field> AddField(string fieldValue)
        {
            var field = new Field()
            {
                Value = fieldValue
            };
            await fieldRepository.AddAsync(field);

            return field;
        }

        public List<Field> GetAll()
        {
            return fieldRepository.GetAll().ToList();
        }

        public async Task<IEnumerable<Follow>> GetFollower(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq("to_id", request.UserId);
            var queryable = (await followRepository.FindListAsync(findFilter)).AsQueryable();

            if (request.FromDate != null)
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            if (request.ToDate != null)
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);

            if (request.OrderType == OrderType.Ascending)
                queryable = queryable.OrderBy(x => x.FollowDate);
            else queryable = queryable.OrderByDescending(x => x.FollowDate);
            if (request.Skip.HasValue && request.Count.HasValue)
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            return queryable;
        }

        public async Task<IEnumerable<Follow>> GetFollowing(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq("from_id", request.UserId);
            var queryable = (await followRepository.FindListAsync(findFilter)).AsQueryable();

            if (request.FromDate != null)
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            if (request.ToDate != null)
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);

            if (request.OrderType == OrderType.Ascending)
                queryable = queryable.OrderBy(x => x.FollowDate);
            else queryable = queryable.OrderByDescending(x => x.FollowDate);
            if (request.Skip.HasValue && request.Count.HasValue)
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            return queryable;
        }

        public async Task<IEnumerable<User>> FilterUser(FilterUserRequest request)
        {
            var users = userRepository.GetAll().AsQueryable();
            if (!String.IsNullOrEmpty(request.KeyWord))
                users = users.Where(x => x.Email.Contains(request.KeyWord)
                || x.FirstName.Contains(request.KeyWord)
                || x.LastName.Contains(request.KeyWord)
                || x.PhoneNumber.Contains(request.KeyWord));

            //if (!string.IsNullOrEmpty(request.Fields))
            //{
            //    var tempField = await fieldRepository.GetByIdAsync(ObjectId.Parse(request.Fields));
            //    users = users.Where(x => x.Fortes.Contains(tempField));
            //}

            if (request.FilterType.HasValue && request.OrderType.HasValue)
            {
                switch (request.FilterType.Value)
                {
                    case UserFilterType.PostCount:
                        {
                            if (request.OrderType.Value == OrderTypeUser.Ascending)
                                users = users.OrderBy(x => x.PostCount);
                            else users = users.OrderByDescending(x => x.PostCount);
                            break;
                        }
                    case UserFilterType.Follower:
                        {
                            if (request.OrderType.Value == OrderTypeUser.Ascending)
                                users = users.OrderBy(x => x.Followers);
                            else users = users.OrderByDescending(x => x.Followers);
                            break;
                        }
                    default:
                        break;
                }
            }

            if (request.Skip.HasValue && request.Count.HasValue)
                users = users.Skip(request.Skip.Value).Take(request.Count.Value);
            return users;
        }

       

        public async Task<User> AddInfo(List<IDictionary<string, string>> request)
        {
            var currentuser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            currentuser.AdditionalInfos.AddRange(request);
            await userRepository.UpdateAsync(currentuser, currentuser.Id);

            return currentuser;
        }
    }
}


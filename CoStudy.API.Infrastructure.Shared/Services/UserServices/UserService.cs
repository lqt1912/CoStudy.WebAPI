using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant.FollowConstant;
using static Common.Constant.UserConstant;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    /// <summary>
    /// The User Service.
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.UserServices.IUserService" />
    /// <seealso cref="IUserService" />
    public class UserService : IUserService
    {
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The account repository
        /// </summary>
        IAccountRepository accountRepository;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration _configuration;
        /// <summary>
        /// The post repository
        /// </summary>
        IPostRepository postRepository;
        /// <summary>
        /// The client group repository
        /// </summary>
        IClientGroupRepository clientGroupRepository;
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;
        /// <summary>
        /// The follow repository
        /// </summary>
        IFollowRepository followRepository;

        /// <summary>
        /// The object level repository
        /// </summary>
        IObjectLevelRepository objectLevelRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;
        private INotificationObjectRepository notificationObjectRepository;
        private IFcmRepository fcmRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="fieldRepository">The field repository.</param>
        /// <param name="followRepository">The follow repository.</param>
        /// <param name="mapper">The mapper.</param>
        public UserService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            IPostRepository postRepository,
            IClientGroupRepository clientGroupRepository,
            IFieldRepository fieldRepository, IFollowRepository followRepository, IMapper mapper, IObjectLevelRepository objectLevelRepository, INotificationObjectRepository notificationObjectRepository, IFcmRepository fcmRepository)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            this.accountRepository = accountRepository;
            this.postRepository = postRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.fieldRepository = fieldRepository;
            this.followRepository = followRepository;
            this.mapper = mapper;
            this.objectLevelRepository = objectLevelRepository;
            this.notificationObjectRepository = notificationObjectRepository;
            this.fcmRepository = fcmRepository;
        }


        /// <summary>
        /// Adds the avatar asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<UserViewModel> AddAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = avatar.ImageHash;
            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return mapper.Map<UserViewModel>(currentUser);

        }


        /// <summary>
        /// Adds the followings asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<string> AddFollowingsAsync(AddFollowerRequest request)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);

            foreach (var item in request.Followers)
            {
                var user = await userRepository.GetByIdAsync(ObjectId.Parse(item));
                if (user != null)
                {
                    var filter = Builders<Follow>.Filter;
                    var finder = filter.Eq(FromId, currentUser.OId) & filter.Eq(ToId, item);
                    var existFollowing = await followRepository.FindAsync(finder);
                    if (existFollowing != null)
                    {
                        return UserFollowAlready;
                    }

                    var follow = new Follow()
                    {
                        FromId = currentUser.OId,
                        ToId = item,
                        FollowDate = DateTime.Now
                    };

                    await followRepository.AddAsync(follow);


                    var notificationObjectBuilders = Builders<NotificationObject>.Filter;

                    var notificationObjectFilters = notificationObjectBuilders.Eq("object_id", item)
                        & notificationObjectBuilders.Eq("notification_type", "FOLLOW_NOTIFY");

                    var existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                    string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                    if (existNotificationObject == null)
                    {
                        var newNotificationObject = new NotificationObject()
                        {
                            NotificationType = "FOLLOW_NOTIFY",
                            ObjectId = currentUser.OId,
                            OwnerId = item
                        };
                        await notificationObjectRepository.AddAsync(newNotificationObject);
                        notificationObject = newNotificationObject.OId;
                    }

                    var notificationDetail = new NotificationDetail()
                    {
                        CreatorId = currentUser.OId,
                        NotificationObjectId = notificationObject
                    };

                    await fcmRepository.PushNotifyDetail(item, notificationDetail);

                }
            }
            return FollowSuccess;
        }

        /// <summary>
        /// Adds the user asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<UserViewModel> AddUserAsync(AddUserRequest entity)
        {
            var phoneFilter = Builders<User>.Filter.Eq("phone_number", entity.PhoneNumber);
            var existUser = await userRepository.FindAsync(phoneFilter);
            if (existUser != null)
                throw new Exception("Số điện thoại này đã được đăng kí. ");
            var user = UserAdapter.FromRequest(entity);
            await userRepository.AddAsync(user);
            return mapper.Map<UserViewModel>(user);
        }


        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy user</exception>
        public async Task<UserViewModel> GetUserById(string id)

        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(id));
            if (user == null)
            {
                throw new Exception(UserNotFound);
            }

            return mapper.Map<UserViewModel>(user);
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy user
        /// or
        /// Không tìm thấy user</exception>
        public UserViewModel GetCurrentUser()
        {
            try
            {
                var user = Feature.CurrentUser(_httpContextAccessor, userRepository);

                if (user == null)
                {
                    throw new Exception(UserNotFound);
                }

                return mapper.Map<UserViewModel>(user);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the following.
        /// </summary>
        /// <param name="toFollowerId">To follower identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Người dùng không tồn tại. Đã có lổi xảy ra</exception>
        public async Task<string> RemoveFollowing(string toFollowerId)
        {
            try
            {
                var findFilter = Builders<Follow>.Filter.Eq(ToId, toFollowerId);
                var following = await followRepository.FindAsync(findFilter);
                var followViewModel = mapper.Map<FollowViewModel>(following);
                await followRepository.DeleteAsync(following.Id);
                return $"{UserUnfollow}{followViewModel.ToName}";
            }
            catch (Exception)
            {
                throw new Exception(UserNotFound);
            }
        }

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<UserViewModel> UpdateUserAsync(UpdateUserRequest request)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            currentUser.Address = request.Address;
            currentUser.FirstName = request.FisrtName;
            currentUser.LastName = request.LastName;
            currentUser.PhoneNumber = request.PhoneNumber;
            currentUser.DateOfBirth = request.DateOfBirth;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return mapper.Map<UserViewModel>(currentUser);
        }

        /// <summary>
        /// Updates the avatar asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<UserViewModel> UpdateAvatarAsync(AddAvatarRequest request)
        {
            var avatar = UserAdapter.FromRequest(request, _httpContextAccessor);

            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);

            currentUser.Avatar = avatar;
            currentUser.AvatarHash = request.AvatarHash;

            foreach (var post in postRepository.GetAll()
                .Where(x => x.AuthorId == currentUser.Id.ToString()
                && x.Status == ItemStatus.Active))
            {
                await postRepository.UpdateAsync(post, post.Id);
            }

            currentUser.ModifiedDate = DateTime.Now;

            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            //CacheHelper.Delete($"CurrentUser-{currentUser.Email}");
            //CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));

            return mapper.Map<UserViewModel>(currentUser);
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <returns></returns>
        public async Task<Field> AddField(string fieldValue)
        {
            var field = new Field()
            {
                Value = fieldValue
            };
            await fieldRepository.AddAsync(field);

            return field;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public List<Field> GetAll()
        {
            return fieldRepository.GetAll().ToList();
        }

        /// <summary>
        /// Gets the follower.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<FollowViewModel>> GetFollower(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq(ToId, request.UserId);
            var queryable = mapper.Map<List<FollowViewModel>>(await followRepository.FindListAsync(findFilter)).AsQueryable();

            queryable = queryable.Where(x => x.FromName.NormalizeSearch().Contains(request.KeyWord.NormalizeSearch()));

            if (request.FromDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            }

            if (request.ToDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);
            }

            if (request.OrderType == SortType.Ascending)
            {
                queryable = queryable.OrderBy(x => x.FollowDate);
            }
            else
            {
                queryable = queryable.OrderByDescending(x => x.FollowDate);
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return queryable.ToList();
        }

        /// <summary>
        /// Gets the following.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<FollowViewModel>> GetFollowing(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq(FromId, request.UserId);
            IQueryable<FollowViewModel> queryable = mapper.Map<List<FollowViewModel>>(await followRepository.FindListAsync(findFilter)).AsQueryable();

            queryable = queryable.Where(x => x.ToName.NormalizeSearch().Contains(request.KeyWord.NormalizeSearch()));
            if (request.FromDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            }

            if (request.ToDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);
            }

            if (request.OrderType == SortType.Ascending)
            {
                queryable = queryable.OrderBy(x => x.FollowDate);
            }
            else
            {
                queryable = queryable.OrderByDescending(x => x.FollowDate);
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return queryable.ToList();
        }

        /// <summary>
        /// Filters the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserViewModel>> FilterUser(FilterUserRequest request)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Regex(FirstName, request.KeyWord)
                            | builder.Regex(LastName, request.KeyWord)
                            | builder.Regex(Email, request.KeyWord)
                            | builder.Regex(PhoneNumber, request.KeyWord);
            var users = (await userRepository.FindListAsync(filter)).AsQueryable();

            var user = new List<User>();

            foreach (var item in users)
            {
                if (IsUserMatchField(item, request.FieldFilter) == true)
                {
                    user.Add(item);
                }
            }

            var userViewModel = mapper.Map<List<UserViewModel>>(user);
            if (request.FilterType.HasValue && request.OrderType.HasValue)
            {
                switch (request.FilterType.Value)
                {
                    case UserFilterType.PostCount:
                        {
                            if (request.OrderType.Value == OrderTypeUser.Descending)
                            {
                                userViewModel = userViewModel.OrderByDescending(x => x.PostCount).ToList();
                            }
                            else
                            {
                                userViewModel = userViewModel.OrderBy(x => x.PostCount).ToList();
                            }

                            break;
                        }
                    case UserFilterType.Follower:
                        {
                            if (request.OrderType.Value == OrderTypeUser.Descending)
                            {
                                userViewModel = userViewModel.OrderByDescending(x => x.Followers).ToList();
                            }
                            else
                            {
                                userViewModel = userViewModel.OrderBy(x => x.Followers).ToList();
                            }

                            break;
                        }
                }
            }

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                userViewModel = userViewModel.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
            }
            return userViewModel;

        }


        /// <summary>
        /// Adds the or update information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<UserViewModel> AddOrUpdateInfo(List<AdditionalInfomation> request)
        {
            var currentuser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            currentuser.AdditionalInfos.Clear();
            currentuser.AdditionalInfos.AddRange(request);
            await userRepository.UpdateAsync(currentuser, currentuser.Id);
            return mapper.Map<UserViewModel>(currentuser);
        }

        private bool IsUserMatchField(User user, IEnumerable<string> fieldFilter)
        {
            var objlvls = objectLevelRepository.GetAll().Where(x => x.ObjectId == user.OId).Select(x => x.FieldId);

            if (fieldFilter.Count() <= 0)
            {
                return true;
            }

            foreach (var f in fieldFilter)
            {
                if (string.IsNullOrEmpty(f))
                {
                    return true;
                }
            }
            foreach (var item in objlvls)
            {
                if (fieldFilter.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<UserNearbyViewModel> GetNearbyUser(BaseGetAllRequest baseGetAllRequest)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            var currentLocation = new Location()
            {
                Latitude = Feature.ParseDouble(currentUser.Address.Latitude),
                Longitude = Feature.ParseDouble(currentUser.Address.Longtitude)

            };
            var userNearbys = new List<UserNearbyViewModel>();
            foreach (var x in userRepository.GetAll())
            {
                var a = new UserNearbyViewModel
                {
                    UserId = x.OId,
                    FullName = $"{x.FirstName} {x.LastName}",
                    Avatar = x.AvatarHash,
                    Location = new Location()
                    {
                        Longitude = Feature.ParseDouble(x.Address.Longtitude),
                        Latitude = Feature.ParseDouble(x.Address.Latitude)
                    },
                    Distance = 0.0
                };
                a.Distance = Math.Round((Feature.CalculateDistance(currentLocation, a.Location) * 1.0 / 1000), 2);
                userNearbys.Add(a);
            }


            if (baseGetAllRequest.Skip.HasValue && baseGetAllRequest.Count.HasValue)
            {
                userNearbys = userNearbys.Skip(baseGetAllRequest.Skip.Value).Take(baseGetAllRequest.Count.Value).ToList();
            }

            return userNearbys;
        }


        public async Task<UserViewModel> AddOrUpdateCallId(AddOrUpdateCallIdRequest request)
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                user.CallId = request.CallId;
                await userRepository.UpdateAsync(user, user.Id);
                return mapper.Map<UserViewModel>(user);
            }

            else throw new Exception();
        }

        public async Task<bool> IsEmailExist(string email)
        {
            var userBuilder = Builders<User>.Filter;
            var userFilter = userBuilder.Eq("email", email) & userBuilder.Eq("status", ItemStatus.Active);
            var user = await userRepository.FindAsync(userFilter);
            if (user != null)
                return true;
            return false;
        }

        public async Task<UserViewModel> UpdateUserAddress(Address request)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);
            currentUser.Address = request;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);
            return mapper.Map<UserViewModel>(currentUser);
        }

        public async Task<UserViewModel> ModifiedUser(ModifiedRequest request)
        {
            var userBuilders = Builders<User>.Filter;
            var userFilter = userBuilders.Eq("oid", request.UserId);
            var user = await userRepository.FindAsync(userFilter);
            if (user == null)
                throw new Exception("Không tìm thấy người dùng. ");
            user.Status = request.Status;
            await userRepository.UpdateAsync(user, user.Id);
            return mapper.Map<UserViewModel>(user);
        }
    }
}


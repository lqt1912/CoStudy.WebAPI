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
using static Common.Constants;

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

        IObjectLevelRepository objectLevelRepository;

        IMapper mapper;
        private INotificationObjectRepository notificationObjectRepository;
        private IFcmRepository fcmRepository;
        ILevelRepository levelRepository;
        public UserService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IAccountRepository accountRepository,
            IPostRepository postRepository,
            IClientGroupRepository clientGroupRepository,
            IFieldRepository fieldRepository, IFollowRepository followRepository, IMapper mapper, IObjectLevelRepository objectLevelRepository, INotificationObjectRepository notificationObjectRepository, IFcmRepository fcmRepository, ILevelRepository levelRepository)
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
            this.levelRepository = levelRepository;
        }


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

                    var notificationDetail = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = item,
                        ObjectId = currentUser.OId,
                        ObjectThumbnail = string.Empty
                    };

                    await fcmRepository.PushNotify(item, notificationDetail, NotificationContent.FollowNotification);
                }
            }
            return FollowSuccess;
        }

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


        public async Task<UserViewModel> GetUserById(string id)

        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(id));
            if (user == null)
            {
                throw new Exception(UserNotFound);
            }

            return mapper.Map<UserViewModel>(user);
        }

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

        public async Task UpdateUserPoint(string userId, string fieldId, int point)
        {
            var builder = Builders<ObjectLevel>.Filter;
            var filter = builder.Eq("object_id", userId)
                & builder.Eq("field_id", fieldId)
                & builder.Eq("is_active", true);
            var objectLevel = await objectLevelRepository.FindAsync(filter);

            if (objectLevel != null)
            {
                objectLevel.Point += point;
                if (objectLevel.Point < LevelPoint.Level1)
                {
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 0)?.OId;
                }
                else if (objectLevel.Point < LevelPoint.Level2)
                {
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 1)?.OId;
                }
                else if (objectLevel.Point < LevelPoint.Level3)
                {
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 2)?.OId;
                }
                else if (objectLevel.Point < LevelPoint.Level4)
                {
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 3)?.OId;
                }
                else
                {
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 4)?.OId;
                }

                await objectLevelRepository.UpdateAsync(objectLevel, objectLevel.Id);
            }
        }

        public async Task AddPoint(string userId, string postId)
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(userId));

            var post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
            if (post == null || post.Status != ItemStatus.Active)
                return;

            var builderUser = Builders<ObjectLevel>.Filter;
            var filterUser = builderUser.Eq("object_id", userId)
                                & builderUser.Eq("is_active", true);
            var userObjectLevels = await objectLevelRepository.FindListAsync(filterUser);

            var builderPost = Builders<ObjectLevel>.Filter;
            var filterPost = builderPost.Eq("object_id", postId)
                            & builderPost.Eq("is_active", true);
            var postObjectLevels = await objectLevelRepository.FindListAsync(filterPost);
            if (postObjectLevels.Count <= 0)
            {
                userObjectLevels.ForEach(async x =>
                {
                    await UpdateUserPoint(userId, x.FieldId, 1);
                });
            }

            userObjectLevels.ForEach(x =>
            {
                postObjectLevels.ForEach(async y =>
                {
                    if (y.FieldId == x.FieldId)
                    {
                        var level = await levelRepository.GetByIdAsync(ObjectId.Parse(y.LevelId));
                        if (level != null)
                        {
                            switch (level.Order)
                            {
                                case 0:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Level1);
                                    break;
                                case 1:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Level2);
                                    break;
                                case 2:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Level3);
                                    break;
                                case 3:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Level4);
                                    break;
                                case 4:
                                case 5:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Level5);
                                    break;
                                default:
                                    await UpdateUserPoint(userId, x.FieldId, PointAdded.Default);
                                    break;
                            }
                        }
                    }
                });
            });
        }

    }
}


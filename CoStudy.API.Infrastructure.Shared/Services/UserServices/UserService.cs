using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using static Common.Constant.FollowConstant;
using static Common.Constant.UserConstant;

namespace CoStudy.API.Infrastructure.Shared.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository postRepository;
        private readonly IFieldRepository fieldRepository;
        private readonly IFollowRepository followRepository;
        private readonly IObjectLevelRepository objectLevelRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IPostRepository postRepository, 
            IFieldRepository fieldRepository,
            IFollowRepository followRepository, 
            IMapper mapper, 
            IObjectLevelRepository objectLevelRepository)
        {
            this.userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            this.postRepository = postRepository;
            this.fieldRepository = fieldRepository;
            this.followRepository = followRepository;
            this.mapper = mapper;
            this.objectLevelRepository = objectLevelRepository;
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
                if (user == null) continue;

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
            }

            return FollowSuccess;
        }

        public async Task<UserViewModel> AddUserAsync(AddUserRequest entity)
        {
            var phoneBuilder = Builders<User>.Filter;
            var phoneFilter = phoneBuilder.Eq("phone_number", entity.PhoneNumber);
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
                throw new Exception(UserNotFound);
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
            currentUser.FirstName = request.FisrtName;
            currentUser.LastName = request.LastName;
            currentUser.PhoneNumber = request.PhoneNumber;
            currentUser.DateOfBirth = request.DateOfBirth;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);
            return mapper.Map<UserViewModel>(currentUser);
        }

        public async Task<UserViewModel> UpdateUserAddress(Address request)
        {
            var currentUser = Feature.CurrentUser(_httpContextAccessor, userRepository);

            currentUser.Address ??= new Address();

            if (!string.IsNullOrEmpty(request.City))
                currentUser.Address.City = request.City;
            if (!string.IsNullOrEmpty(request.District))
                currentUser.Address.District = request.District;
            if (!string.IsNullOrEmpty(request.Detail))
                currentUser.Address.Detail = request.Detail;
            if (!string.IsNullOrEmpty(request.Latitude) && !string.IsNullOrEmpty(request.Longtitude))
            {
                currentUser.Address.Latitude = request.Latitude;
                currentUser.Address.Longtitude = request.Longtitude;
            }

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
                .Where(x => x.AuthorId == currentUser.Id.ToString() && x.Status == ItemStatus.Active))
            {
                await postRepository.UpdateAsync(post, post.Id);
            }

            currentUser.ModifiedDate = DateTime.Now;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return mapper.Map<UserViewModel>(currentUser);
        }

        public async Task<Field> AddField(string fieldValue)
        {
            var field = new Field() {Value = fieldValue};
            await fieldRepository.AddAsync(field);
            return field;
        }

        public List<Field> GetAll()
        {
            return fieldRepository.GetAll().ToList().Where(x => x.Status == ItemStatus.Active).ToList();
        }

        public async Task<IEnumerable<FollowViewModel>> GetFollower(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq(ToId, request.UserId);
            var queryable = mapper.Map<List<FollowViewModel>>(await followRepository.FindListAsync(findFilter))
                .AsQueryable();
            queryable = queryable.Where(x => x.FromName.NormalizeSearch().Contains(request.KeyWord.NormalizeSearch()));
            if (request.FromDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            }

            if (request.ToDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);
            }

            queryable = request.OrderType switch
            {
                SortType.Ascending => queryable.OrderBy(x => x.FollowDate),
                _ => queryable.OrderByDescending(x => x.FollowDate)
            };

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return queryable.ToList();
        }

        public async Task<IEnumerable<FollowViewModel>> GetFollowing(FollowFilterRequest request)
        {
            var findFilter = Builders<Follow>.Filter.Eq(FromId, request.UserId);
            var queryable = mapper
                .Map<List<FollowViewModel>>(await followRepository.FindListAsync(findFilter)).AsQueryable();
            queryable = queryable.Where(x => x.ToName.NormalizeSearch().Contains(request.KeyWord.NormalizeSearch()));
            if (request.FromDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate >= request.FromDate);
            }

            if (request.ToDate != null)
            {
                queryable = queryable.Where(x => x.FollowDate <= request.ToDate);
            }

            queryable = request.OrderType switch
            {
                SortType.Ascending => queryable.OrderBy(x => x.FollowDate),
                _ => queryable.OrderByDescending(x => x.FollowDate)
            };

            if (request.Skip.HasValue && request.Count.HasValue)
            {
                queryable = queryable.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return queryable.ToList();
        }

        public async Task<IEnumerable<UserViewModel>> FilterUser(FilterUserRequest request)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Regex(FirstName, request.KeyWord) | builder.Regex(LastName, request.KeyWord) |
                         builder.Regex(Email, request.KeyWord) | builder.Regex(PhoneNumber, request.KeyWord);
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
                        if (request.OrderType.Value != OrderTypeUser.Descending)
                        {
                            userViewModel = userViewModel.OrderBy(x => x.PostCount).ToList();
                        }
                        else
                        {
                            userViewModel = userViewModel.OrderByDescending(x => x.PostCount).ToList();
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
                    default:
                        throw new ArgumentOutOfRangeException("Không hỗ trợ bộ lọc này. ");
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
            if (!fieldFilter.Any())
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
                userNearbys = userNearbys.Skip(baseGetAllRequest.Skip.Value).Take(baseGetAllRequest.Count.Value)
                    .ToList();
            }

            return userNearbys;
        }

        public async Task<UserViewModel> AddOrUpdateCallId(AddOrUpdateCallIdRequest request)
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user == null) 
                throw new Exception();

            user.CallId = request.CallId;
            await userRepository.UpdateAsync(user, user.Id);

            return mapper.Map<UserViewModel>(user);

        }

        public async Task<bool> IsEmailExist(string email)
        {
            var userBuilder = Builders<User>.Filter;
            var userFilter = userBuilder.Eq("email", email) & userBuilder.Eq("status", ItemStatus.Active);
            var user = await userRepository.FindAsync(userFilter);
            return user != null;
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
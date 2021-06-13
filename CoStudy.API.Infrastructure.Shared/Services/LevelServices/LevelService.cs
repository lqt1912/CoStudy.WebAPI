using AutoMapper;
using Common;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class LevelService : ILevelService
    {
        ILevelRepository levelRepository;
        IObjectLevelRepository objectLevelRepository;
        IUserRepository userRepository;
        IFieldRepository fieldRepository;

        IPostRepository postRepository;

        IHttpContextAccessor httpContextAccessor;

        IMapper mapper;
        public LevelService(ILevelRepository levelRepository, IObjectLevelRepository objectLevelRepository, IUserRepository userRepository, IFieldRepository fieldRepository, IMapper mapper, IPostRepository postRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.levelRepository = levelRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.userRepository = userRepository;
            this.fieldRepository = fieldRepository;
            this.mapper = mapper;
            this.postRepository = postRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserViewModel> AddOrUpdateUserFields(UserAddFieldRequest request)
        {
            User user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));
            if (user != null)
            {
                FilterDefinition<ObjectLevel> buidler = Builders<ObjectLevel>.Filter.Eq("object_id", request.UserId);

                List<ObjectLevel> existObjectLevels = (await objectLevelRepository.FindListAsync(buidler)).ToList();

                foreach (string item in request.FieldId)
                {
                    var existObjectLevel = existObjectLevels.FirstOrDefault(x => x.FieldId == item);
                    if (existObjectLevel != null)
                    {
                        if (existObjectLevel.IsActive == false)
                        {
                            existObjectLevel.IsActive = true;
                        }

                        existObjectLevel.ModifiedDate = DateTime.Now;
                        await objectLevelRepository.UpdateAsync(existObjectLevel, existObjectLevel.Id);
                    }
                    else
                    {
                        ObjectLevel objectLevel = new ObjectLevel()
                        {
                            LevelId = Constants.LevelConstants.LEVEL_0_ID,
                            FieldId = item,
                            ObjectId = user.OId,
                            Point = 0
                        };
                        await objectLevelRepository.AddAsync(objectLevel);
                    }
                }

                foreach (var ex in existObjectLevels)
                {
                    if (request.FieldId.FirstOrDefault(x => x == ex.FieldId) == null)
                    {
                        ex.IsActive = false;
                        ex.ModifiedDate = DateTime.Now;
                        await objectLevelRepository.UpdateAsync(ex, ex.Id);
                    }
                }
                return mapper.Map<UserViewModel>(user);
            }
            else
            {
                throw new Exception("Không tìm thấy user");
            }
        }

        public async Task<IEnumerable<LevelViewModel>> AddLevel(IEnumerable<Level> level)
        {
            List<Level> result = new List<Level>();

            foreach (Level item in level)
            {
                Level model = new Level()
                {
                    Description = item.Description,
                    Icon = item.Icon,
                    Name = item.Name
                };
                await levelRepository.AddAsync(model);
                result.Add(model);
            }

            return mapper.Map<IEnumerable<LevelViewModel>>(result);
        }

        public async Task<IEnumerable<ObjectLevelViewModel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels)
        {
            List<ObjectLevel> result = new List<ObjectLevel>();
            foreach (ObjectLevel item in objectLevels)
            {
                Level existLevel = await levelRepository.GetByIdAsync(ObjectId.Parse(item.LevelId));

                if (existLevel != null)
                {
                    ObjectLevel model = new ObjectLevel()
                    {
                        LevelId = item.LevelId,
                        ObjectId = item.ObjectId,
                        FieldId = item.FieldId,
                        Point = item.Point
                    };
                    await objectLevelRepository.AddAsync(model);
                    result.Add(model);
                }
            }

            return mapper.Map<IEnumerable<ObjectLevelViewModel>>(result);
        }


        public async Task<LevelViewModel> GetById(string id)
        {
            FilterDefinition<Level> filter = Builders<Level>.Filter.Eq("oid", id);
            Level result = await levelRepository.FindAsync(filter);
            return mapper.Map<LevelViewModel>(result);
        }

        public IEnumerable<LevelViewModel> GetAllLevel(BaseGetAllRequest request)
        {
            IQueryable<Level> result = levelRepository.GetAll().Where(x=>x.IsActive ==true);
            if (request.Skip.HasValue && request.Count.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return mapper.Map<IEnumerable<LevelViewModel>>(result);
        }

        public async Task<IEnumerable<ObjectLevelViewModel>> GetFieldsOfUser(string userId)
        {
            FilterDefinition<ObjectLevel> filter = Builders<ObjectLevel>.Filter.Eq("object_id", userId);

            List<ObjectLevel> objectlevels = (await objectLevelRepository.FindListAsync(filter));
            return mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectlevels);

        }

        public async Task<IEnumerable<ObjectLevelViewModel>> GetLevelByObject(string objectId)
        {
            FilterDefinition<ObjectLevel> filter = Builders<ObjectLevel>.Filter.Eq("object_id", objectId);
            List<ObjectLevel> result = await objectLevelRepository.FindListAsync(filter);
            return mapper.Map<IEnumerable<ObjectLevelViewModel>>(result);
        }


        public bool IsMatch(IEnumerable<ObjectLevelViewModel> userObjectLevels, IEnumerable<ObjectLevelViewModel> postObjectLevels)
        {
            foreach (ObjectLevelViewModel userObjectLevel in userObjectLevels)
            {
                foreach (ObjectLevelViewModel postObjectLevel in postObjectLevels)
                {
                    if ((userObjectLevel.LevelId == postObjectLevel.LevelId) || (userObjectLevel.Point >= postObjectLevel.Point))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllPostMatchUser(string postId)
        {

            List<User> result = new List<User>();
            IEnumerable<ObjectLevelViewModel> postObjectLevels = await GetLevelByObject(postId);

            foreach (User user in userRepository.GetAll())
            {
                IEnumerable<ObjectLevelViewModel> userObjectLevels = await GetLevelByObject(user.OId);
                if (IsMatch(userObjectLevels, postObjectLevels) == true)
                {
                    result.Add(user);
                }
            }

            return result;
        }


        public async Task<ObjectLevelViewModel> AddPoint(AddPointRequest request)
        {
            string objectLevelId = request.ObjectLevelId;
            int point = request.Point;
            ObjectLevel objectLevel = await objectLevelRepository.GetByIdAsync(ObjectId.Parse(objectLevelId));
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

                return mapper.Map<ObjectLevelViewModel>(objectLevel);
            }
            else
            {
                throw new Exception("Không tìm thấy Object Level. ");
            }
        }

        public async Task<UserViewModel> ResetField(UserResetFieldRequest request)
        {
            User user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                foreach (string item in request.FieldId)
                {
                    FilterDefinitionBuilder<ObjectLevel> finder = Builders<ObjectLevel>.Filter;
                    FilterDefinition<ObjectLevel> filter = finder.Eq("field_id", item) & finder.Eq("object_id", user.OId);
                    ObjectLevel objectLevel = await objectLevelRepository.FindAsync(filter);
                    if (objectLevel != null)
                    {
                        objectLevel.LevelId = Constants.LevelConstants.LEVEL_0_ID;
                        objectLevel.Point = 0;
                        objectLevel.ModifiedDate = DateTime.Now;
                        await objectLevelRepository.UpdateAsync(objectLevel, objectLevel.Id);
                    }
                }

                return mapper.Map<UserViewModel>(user);
            }
            else
            {
                throw new Exception("Không tìm thấy user. ");
            }
        }


        public async Task<PostViewModel> UpdatePostField(UpdatePostLevelRequest request)
        {

            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (post != null)
            {
                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                if (currentUser.OId != post.AuthorId)
                {
                    throw new Exception("Bạn không có quyền chỉnh sửa nội dung này. ");
                }

                FilterDefinition<ObjectLevel> finder = Builders<ObjectLevel>.Filter.Eq("object_id", request.PostId);
                List<ObjectLevel> postExistObjectLevel = await objectLevelRepository.FindListAsync(finder);

                foreach (ObjectLevel item in postExistObjectLevel)
                {
                    await objectLevelRepository.DeleteAsync(item.Id);
                }

                foreach (ObjectLevel item in request.Field)
                {
                    item.Point = 0;
                    item.ObjectId = request.PostId;
                    item.CreateDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.IsActive = true;

                    await objectLevelRepository.AddAsync(item);
                }

            }

            return mapper.Map<PostViewModel>(post);

        }

        private async Task<int> GetUserPoint(string userId, string fieldId)
        {
            if (string.IsNullOrEmpty(fieldId))
            {
                var objectLevelBuilder = Builders<ObjectLevel>.Filter;
                var objectLevelFilter = objectLevelBuilder.Eq("object_id", userId) 
                                        & objectLevelBuilder.Eq("is_active", true);
                var objlvls = await objectLevelRepository.FindListAsync(objectLevelFilter);
                var totalPoint = 0;
                objlvls.ForEach(x => { totalPoint = totalPoint + x.Point.Value; });
                return totalPoint;
            }
            else
            {
                var objectLevelBuilder = Builders<ObjectLevel>.Filter;
                var objectLevelFilter = objectLevelBuilder.Eq("object_id", userId) 
                                        & objectLevelBuilder.Eq("is_active", true)
                                        &objectLevelBuilder.Eq("field_id", fieldId);
                var objlvls = await objectLevelRepository.FindAsync(objectLevelFilter);

                if (objlvls == null) return 0;
                return objlvls.Point ?? 0;

            }
        }

        public async Task<LeaderBoardViewModel> GetLeaderBoard(LeaderBoardRequest request)
        {
            var users = userRepository.GetAll();
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var result = new LeaderBoardViewModel();

            foreach (var user in users)
            {
                var userViewModel = new UserLeaderBoardViewModel()
                {
                    TotalPoint = await GetUserPoint(user.OId, request.FieldId),
                    UserAvatar = user.AvatarHash,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserId = user.OId
                };
                result.LeaderBoards.Add(userViewModel);
                if (userViewModel.UserId != currentUser.OId) continue;
                var currentUserLeaderBoard = new CurrentUserLeaderBoardViewModel()
                {
                    TotalPoint = await GetUserPoint(user.OId, request.FieldId),
                    UserAvatar = user.AvatarHash,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserId = user.OId
                };
                result.CurrentUser = currentUserLeaderBoard;
            }

            result.LeaderBoards = result.LeaderBoards.OrderByDescending(x => x.TotalPoint).ToList();
            var iter = 1;
            foreach (var item in result.LeaderBoards)
            {
                item.Index = iter++;
                if (result.CurrentUser.UserId == item.UserId)
                {
                    result.CurrentUser.Index = item.Index;
                }
            }
            return result;
        }
    }
}

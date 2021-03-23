using AutoMapper;
using Common;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Class LevelService
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.ILevelService" />
    public class LevelService : ILevelService
    {
        /// <summary>
        /// The level repository
        /// </summary>
        ILevelRepository levelRepository;
        /// <summary>
        /// The object level repository
        /// </summary>
        IObjectLevelRepository objectLevelRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;

        IPostRepository postRepository;

        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelService" /> class.
        /// </summary>
        /// <param name="levelRepository">The level repository.</param>
        /// <param name="objectLevelRepository">The object level repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="fieldRepository">The field repository.</param>
        /// <param name="mapper">The mapper.</param>
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


        /// <summary>
        /// Adds or update user fields.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Không tìm thấy user</exception>
        /// <exception cref="Exception">Không tìm thấy user</exception>
        public async Task<UserViewModel> AddOrUpdateUserFields(UserAddFieldRequest request)
        {
            User user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                var buidler = Builders<ObjectLevel>.Filter.Eq("object_id", request.UserId);

                var existObjectLevels = (await objectLevelRepository.FindListAsync(buidler)).ToList();

                foreach (var item in request.FieldId)
                {
                    if (existObjectLevels.FirstOrDefault(x => x.FieldId == item) == null)
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
                return mapper.Map<UserViewModel>(user);
            }
            else throw new Exception("Không tìm thấy user");
        }

        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
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

            return mapper.Map<IEnumerable<LevelViewModel>>( result);
        }

        /// <summary>
        /// Adds the object level.
        /// </summary>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<LevelViewModel> GetById(string id)
        {
            FilterDefinition<Level> filter = Builders<Level>.Filter.Eq("oid", id);
            Level result = await levelRepository.FindAsync(filter);
            return  mapper.Map<LevelViewModel>(result);
        }

        /// <summary>
        /// Gets all level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<LevelViewModel> GetAllLevel(BaseGetAllRequest request)
        {
            IQueryable<Level> result = levelRepository.GetAll();
            if (request.Skip.HasValue && request.Count.HasValue)
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            return mapper.Map<IEnumerable<LevelViewModel>>( result);
        }

        /// <summary>
        /// Gets the fields of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy user</exception>
        public async Task<IEnumerable<ObjectLevelViewModel>> GetFieldsOfUser(string userId)
        {
            var filter = Builders<ObjectLevel>.Filter.Eq("object_id", userId);

            var objectlevels = (await objectLevelRepository.FindListAsync(filter));
            return mapper.Map<IEnumerable<ObjectLevelViewModel>>(objectlevels);

        }

        /// <summary>
        /// Gets the level by object.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ObjectLevelViewModel>> GetLevelByObject(string objectId)
        {
            FilterDefinition<ObjectLevel> filter = Builders<ObjectLevel>.Filter.Eq("object_id", objectId);
            List<ObjectLevel> result = await objectLevelRepository.FindListAsync(filter);
            return mapper.Map<IEnumerable<ObjectLevelViewModel>>(result);
        }


        /// <summary>
        /// Determines whether the specified user object levels is match.
        /// </summary>
        /// <param name="userObjectLevels">The user object levels.</param>
        /// <param name="postObjectLevels">The post object levels.</param>
        /// <returns>
        ///   <c>true</c> if the specified user object levels is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(IEnumerable<ObjectLevelViewModel> userObjectLevels, IEnumerable<ObjectLevelViewModel> postObjectLevels)
        {
            foreach (ObjectLevelViewModel userObjectLevel in userObjectLevels)
            {
                foreach (ObjectLevelViewModel postObjectLevel in postObjectLevels)
                {
                    if ((userObjectLevel.LevelId == postObjectLevel.LevelId) || (userObjectLevel.Point >= postObjectLevel.Point))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all post's match user.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllPostMatchUser(string postId)
        {

            List<User> result = new List<User>();
            IEnumerable<ObjectLevelViewModel> postObjectLevels = await GetLevelByObject(postId);

            foreach (User user in userRepository.GetAll())
            {
                IEnumerable<ObjectLevelViewModel> userObjectLevels = await GetLevelByObject(user.OId);
                if (IsMatch(userObjectLevels, postObjectLevels) == true)
                    result.Add(user);
            }

            return result;
        }


        /// <summary>
        /// Adds the point.
        /// </summary>
        /// <param name="objectLevelId">The object level identifier.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Không tìm thấy Object Level.</exception>
        /// <exception cref="Exception">Không tìm thấy Object Level.</exception>
        public async Task<ObjectLevel> AddPoint(string objectLevelId, int point)
        {
            var objectLevel = await objectLevelRepository.GetByIdAsync(ObjectId.Parse(objectLevelId));
            if (objectLevel != null)
            {
                objectLevel.Point += point;
                if (objectLevel.Point > LevelPoint.Level1)
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 0)?.OId;
                else if (objectLevel.Point > LevelPoint.Level2)
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 1)?.OId;
                else if (objectLevel.Point > LevelPoint.Level3)
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 2)?.OId;
                else if (objectLevel.Point > LevelPoint.Level4)
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 3)?.OId;
                else if (objectLevel.Point > LevelPoint.Level5)
                    objectLevel.LevelId = levelRepository.GetAll().FirstOrDefault(x => x.Order == 4)?.OId;

                await objectLevelRepository.UpdateAsync(objectLevel, objectLevel.Id);
                return objectLevel;
            }
            else throw new Exception("Không tìm thấy Object Level. ");
        }

        /// <summary>
        /// Resets the field.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Không tìm thấy user.</exception>
        public async Task<UserViewModel> ResetField(UserResetFieldRequest request )
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                foreach (var item in request.FieldId)
                {
                    var finder = Builders<ObjectLevel>.Filter;
                    var filter = finder.Eq("field_id", item) & finder.Eq("object_id", user.OId);
                    var objectLevel = await objectLevelRepository.FindAsync(filter);
                    if(objectLevel !=null)
                    {
                        objectLevel.LevelId = Constants.LevelConstants.LEVEL_0_ID;
                        objectLevel.Point = 0;
                        objectLevel.ModifiedDate = DateTime.Now;
                        await objectLevelRepository.UpdateAsync(objectLevel, objectLevel.Id);
                    }
                }

                return mapper.Map<UserViewModel>(user);
            }
            else throw new Exception("Không tìm thấy user. ");
        }


        public async Task<PostViewModel> UpdatePostField(UpdatePostLevelRequest request)
        {

            var post = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if(post!=null)
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                if (currentUser.OId != post.AuthorId)
                    throw new Exception("Bạn không có quyền chỉnh sửa nội dung này. ");

                var finder = Builders<ObjectLevel>.Filter.Eq("object_id", request.PostId);
                var postExistObjectLevel = await objectLevelRepository.FindListAsync(finder);

                foreach (var item in postExistObjectLevel)
                    await objectLevelRepository.DeleteAsync(item.Id);

                foreach (var item in request.Field)
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
    }
}

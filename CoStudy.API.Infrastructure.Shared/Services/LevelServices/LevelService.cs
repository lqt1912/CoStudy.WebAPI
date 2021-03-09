using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelService" /> class.
        /// </summary>
        /// <param name="levelRepository">The level repository.</param>
        /// <param name="objectLevelRepository">The object level repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="fieldRepository">The field repository.</param>
        public LevelService(ILevelRepository levelRepository, IObjectLevelRepository objectLevelRepository, IUserRepository userRepository, IFieldRepository fieldRepository)
        {
            this.levelRepository = levelRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.userRepository = userRepository;
            this.fieldRepository = fieldRepository;
        }

        /// <summary>
        /// Adds the fields for user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy user</exception>
        public async Task<User> AddFieldsForUser(UserAddFieldRequest request)
        {
            User user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                ObjectLevel objectLevel = new ObjectLevel()
                {
                    LevelId = request.LevelId,
                    FieldId = request.FieldId,
                    ObjectId = user.OId,
                    Point = 0
                };
                await objectLevelRepository.AddAsync(objectLevel);
                user.Fields.Add(objectLevel.OId);
                await userRepository.UpdateAsync(user, user.Id);
                return user;
            }
            else throw new Exception("Không tìm thấy user");
        }

        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Level>> AddLevel(IEnumerable<Level> level)
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

            return result;
        }

        /// <summary>
        /// Adds the object level.
        /// </summary>
        /// <param name="objectLevels">The object levels.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ObjectLevel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels)
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

            return result;
        }

        /// <summary>
        /// Bets the gy identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Level> BetGyId(string id)
        {
            FilterDefinition<Level> filter = Builders<Level>.Filter.Eq("oid", id);
            Level result = await levelRepository.FindAsync(filter);
            return result;
        }

        /// <summary>
        /// Gets all level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Level> GetAllLevel(BaseGetAllRequest request)
        {
            IQueryable<Level> result = levelRepository.GetAll();
            if (request.Skip.HasValue && request.Count.HasValue)
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            return result;
        }

        /// <summary>
        /// Gets the fields of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy user</exception>
        public async Task<IEnumerable<UserGetFieldResponse>> GetFieldsOfUser(string userId)
        {
            User user = await userRepository.GetByIdAsync(ObjectId.Parse(userId));
            List<UserGetFieldResponse> result = new List<UserGetFieldResponse>();
            if (user != null)
            {
                foreach (string item in user.Fields)
                {
                    ObjectLevel objlvl = await objectLevelRepository.GetByIdAsync(ObjectId.Parse(item));

                    Level level = await levelRepository.GetByIdAsync(ObjectId.Parse(objlvl.LevelId));
                    Field field = await fieldRepository.GetByIdAsync(ObjectId.Parse(objlvl.FieldId));
                    result.Add(new UserGetFieldResponse()
                    {
                        Field = field,
                        Level = level,
                        Point = objlvl.Point,
                        UserId = user.OId
                    });
                }
                return result;
            }
            else throw new Exception("Không tìm thấy user");

        }

        /// <summary>
        /// Gets the level by object.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ObjectLevel>> GetLevelByObject(string objectId)
        {
            FilterDefinition<ObjectLevel> filter = Builders<ObjectLevel>.Filter.Eq("object_id", objectId);
            List<ObjectLevel> result = await objectLevelRepository.FindListAsync(filter);
            return result;
        }


        /// <summary>
        /// Determines whether the specified user object levels is match.
        /// </summary>
        /// <param name="userObjectLevels">The user object levels.</param>
        /// <param name="postObjectLevels">The post object levels.</param>
        /// <returns>
        ///   <c>true</c> if the specified user object levels is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(IEnumerable<ObjectLevel> userObjectLevels, IEnumerable<ObjectLevel> postObjectLevels)
        {
            foreach (ObjectLevel userObjectLevel in userObjectLevels)
            {
                foreach (ObjectLevel postObjectLevel in postObjectLevels)
                {
                    if ((userObjectLevel.LevelId == postObjectLevel.LevelId) && (userObjectLevel.Point >= postObjectLevel.Point))
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
            IEnumerable<ObjectLevel> postObjectLevels = await GetLevelByObject(postId);

            foreach (User user in userRepository.GetAll())
            {
                IEnumerable<ObjectLevel> userObjectLevels = await GetLevelByObject(user.OId);
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


    }
}

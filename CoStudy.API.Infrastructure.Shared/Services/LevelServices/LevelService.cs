using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class LevelService : ILevelService
    {
        ILevelRepository levelRepository;
        IObjectLevelRepository objectLevelRepository;
        IUserRepository userRepository;
        IFieldRepository fieldRepository;
        public LevelService(ILevelRepository levelRepository, IObjectLevelRepository objectLevelRepository, IUserRepository userRepository, IFieldRepository fieldRepository)
        {
            this.levelRepository = levelRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.userRepository = userRepository;
            this.fieldRepository = fieldRepository;
        }

        public async Task<User> AddFieldsForUser(UserAddFieldRequest request)
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(request.UserId));

            if (user != null)
            {
                var objectLevel = new ObjectLevel()
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

        public async Task<IEnumerable<Level>> AddLevel(IEnumerable<Level> level)
        {
            var result = new List<Level>();

            foreach (var item in level)
            {
                var model = new Level()
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

        public async Task<IEnumerable<ObjectLevel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels)
        {
            var result = new List<ObjectLevel>();
            foreach (var item in objectLevels)
            {
                var existLevel = await levelRepository.GetByIdAsync(ObjectId.Parse(item.LevelId));
                
                if(existLevel!=null)
                { 
                var model = new ObjectLevel()
                {
                    LevelId = item.LevelId,
                    ObjectId = item.ObjectId,
                    FieldId = item.FieldId
                };
                await objectLevelRepository.AddAsync(model);
                result.Add(model);
                }
            }

            return result;
        }

        public async Task<Level> BetGyId(string id)
        {
            var filter = Builders<Level>.Filter.Eq("OId", id);
            var result = await levelRepository.FindAsync(filter);
            return result;
        }

        public IEnumerable<Level> GetAllLevel(BaseGetAllRequest request)
        {
            var result = levelRepository.GetAll();
            if (request.Skip.HasValue && request.Count.HasValue)
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            return result;
        }

        public async Task<IEnumerable<UserGetFieldResponse>> GetFieldsOfUser(string userId)
        {
            var user = await userRepository.GetByIdAsync(ObjectId.Parse(userId));
            var result = new List<UserGetFieldResponse>();
            if (user != null)
            {
                foreach (var item in user.Fields)
                {
                    ObjectLevel objlvl = await objectLevelRepository.GetByIdAsync(ObjectId.Parse(item));

                    var level = await levelRepository.GetByIdAsync(ObjectId.Parse(objlvl.LevelId));
                    var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(objlvl.FieldId));
                    result.Add( new UserGetFieldResponse()
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

        public async  Task<IEnumerable<ObjectLevel>> GetLevelByObject(string objectId)
        {
            var filter = Builders<ObjectLevel>.Filter.Eq("object_id", objectId);
            var result = await objectLevelRepository.FindListAsync(filter);
            return result;
        }
    }
}

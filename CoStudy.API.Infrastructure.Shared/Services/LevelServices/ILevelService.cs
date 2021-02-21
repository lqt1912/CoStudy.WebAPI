using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface ILevelService
    {
        Task<IEnumerable<Level>> AddLevel(IEnumerable<Level> level);
        IEnumerable<Level> GetAllLevel(BaseGetAllRequest request);
        Task<Level> BetGyId(string id);

        Task<IEnumerable<ObjectLevel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels);

        Task<IEnumerable<ObjectLevel>> GetLevelByObject(string objectId);

                

        Task<User> AddFieldsForUser(UserAddFieldRequest request);

        Task<IEnumerable<UserGetFieldResponse>> GetFieldsOfUser(string userId);
    }
}

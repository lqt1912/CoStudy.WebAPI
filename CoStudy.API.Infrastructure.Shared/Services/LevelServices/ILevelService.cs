using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface ILevelService
    {
             Task<UserViewModel> AddOrUpdateUserFields(UserAddFieldRequest request);

             Task<IEnumerable<LevelViewModel>> AddLevel(IEnumerable<Level> level);

             Task<IEnumerable<ObjectLevelViewModel>> AddObjectLevel(IEnumerable<ObjectLevel> objectLevels);

             Task<LevelViewModel> GetById(string id);

             IEnumerable<LevelViewModel> GetAllLevel(BaseGetAllRequest request);
             Task<IEnumerable<ObjectLevelViewModel>> GetFieldsOfUser(string userId);

             Task<IEnumerable<ObjectLevelViewModel>> GetLevelByObject(string objectId);

                bool IsMatch(IEnumerable<ObjectLevelViewModel> userObjectLevels, IEnumerable<ObjectLevelViewModel> postObjectLevels);

             Task<IEnumerable<User>> GetAllPostMatchUser(string postId);

             Task<ObjectLevelViewModel> AddPoint(AddPointRequest request);

             Task<UserViewModel> ResetField(UserResetFieldRequest request);

             Task<PostViewModel> UpdatePostField(UpdatePostLevelRequest request);

             Task<LeaderBoardViewModel> GetLeaderBoard(LeaderBoardRequest request);
    }
}

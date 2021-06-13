using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Application;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface IMasterDataServices
    {
        TableResultJson<FieldViewModel> GetAllField(TableRequest request);
        Task<FieldViewModel> GetFieldById(string fieldId);
        TableResultJson<LevelViewModel> GetAllLevel(TableRequest request);
        Task<LevelViewModel> GetLevelById(string levelId);
        TableResultJson<FieldGroupViewModel> GetAllFieldGroup(TableRequest request);
        Task<LevelViewModel> UpdateLevel(Level entity);
        Task<LevelViewModel> AddLevel(Level entity);
        Task<FieldViewModel> UpdateField(Field entity);
    }
}

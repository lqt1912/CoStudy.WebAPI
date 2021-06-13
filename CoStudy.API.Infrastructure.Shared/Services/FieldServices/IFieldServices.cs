using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface IFieldServices
    {
        Task<Field> AddField(Field entity);

        IEnumerable<Field> GetAll(BaseGetAllRequest request);
        Task<string> Delete(String id);

        Task<IEnumerable<Field>> GetFieldByGroupId(string groupId);

        Task<FieldGroupViewModel> AddFieldToGroup(AddFieldToGroupRequest request);

        Task<FieldGroupViewModel> AddFieldGroup(FieldGroup fieldGroup);

        Task<FieldGroupViewModel> RemoveFieldFromGroup(AddFieldToGroupRequest request);

        List<FieldGroupViewModel> GetAllFieldGroup(BaseGetAllRequest request);
    }
}

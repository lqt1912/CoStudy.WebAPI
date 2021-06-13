using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class FieldServices : IFieldServices
    {
        IFieldRepository fieldRepository;

        IFieldGroupRepository fieldGroupRepository;

        IMapper mapper;

        public FieldServices(IFieldRepository fieldRepository, IFieldGroupRepository fieldGroupRepository, IMapper mapper)
        {
            this.fieldRepository = fieldRepository;
            this.fieldGroupRepository = fieldGroupRepository;
            this.mapper = mapper;
        }

        public async Task<Field> AddField(Field entity)
        {
            var data = new Field()
            {
                Value = entity.Value
            };

            await fieldRepository.AddAsync(data);
            return data;
        }

        public IEnumerable<Field> GetAll(BaseGetAllRequest request)
        {
            var data = fieldRepository.GetAll().Where(x=>x.Status ==ItemStatus.Active);

            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }

        public async Task<string> Delete(String id)
        {
            try
            {
                await fieldRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch (Exception)
            {
                throw new Exception("Xóa thất bại.");
            }
        }

        public async Task<FieldGroupViewModel> AddFieldToGroup(AddFieldToGroupRequest request)
        {

            var group = await fieldGroupRepository.GetByIdAsync(ObjectId.Parse(request.GroupId));
            if (group == null)
            {
                throw new Exception("Không tìm thấy nhóm lĩnh vực. ");
            }

            group.GroupName = request.GroupName;
            group.FieldId.Clear();
            foreach (var fieldId in request.FieldIds)
            {
                var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
                if (field != null)
                {
                    group.FieldId.Add(fieldId);
                }
            }
            await fieldGroupRepository.UpdateAsync(group, group.Id);
            return mapper.Map<FieldGroupViewModel>(group);
        }

        public async Task<FieldGroupViewModel> RemoveFieldFromGroup(AddFieldToGroupRequest request)
        {
            var group = await fieldGroupRepository.GetByIdAsync(ObjectId.Parse(request.GroupId));
            if (group == null)
                throw new Exception("Không tìm thấy nhóm lĩnh vực. ");

            foreach (var item in request.FieldIds)
            {
                if (group.FieldId.Any(x => x == item))
                    group.FieldId.Remove(item);
            }
            group.FieldId.Distinct();
            await fieldGroupRepository.UpdateAsync(group, group.Id);
            return mapper.Map<FieldGroupViewModel>(group);
        }

        public async Task<FieldGroupViewModel> AddFieldGroup(FieldGroup fieldGroup)
        {
            var data = new FieldGroup()
            {
                GroupName = fieldGroup.GroupName,
                FieldId = fieldGroup.FieldId
            };

            await fieldGroupRepository.AddAsync(data);
            return mapper.Map<FieldGroupViewModel>(data);
        }

        public List<FieldGroupViewModel> GetAllFieldGroup(BaseGetAllRequest request)
        {
            var data = fieldGroupRepository.GetAll();
            if (request.Skip.HasValue && request.Count.HasValue)
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            return mapper.Map<List<FieldGroupViewModel>>(data);
        }

        public async Task<IEnumerable<Field>> GetFieldByGroupId(string groupId)
        {
            var group = await fieldGroupRepository.GetByIdAsync(ObjectId.Parse(groupId));
            if (group == null)
            {
                throw new Exception("Không tìm thấy nhóm lĩnh vực. ");
            }

            var groupViewModel = mapper.Map<FieldGroupViewModel>(group);

            return groupViewModel.Fields;

        }
    }
}

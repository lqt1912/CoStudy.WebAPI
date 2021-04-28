using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.FieldRequest;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Field Service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IFieldServices" />
    public class FieldServices:IFieldServices
    {
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;

        /// <summary>
        /// The field group repository
        /// </summary>
        IFieldGroupRepository fieldGroupRepository;

        IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldServices"/> class.
        /// </summary>
        /// <param name="fieldRepository">The field repository.</param>
        public FieldServices(IFieldRepository fieldRepository, IFieldGroupRepository fieldGroupRepository, IMapper mapper)
        {
            this.fieldRepository = fieldRepository;
            this.fieldGroupRepository = fieldGroupRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Field> AddField(Field entity)
        {
            var data = new Field()
            {
                Value = entity.Value
            };

            await fieldRepository.AddAsync(data);
            return data;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Field> GetAll(BaseGetAllRequest request)
        {
            var data = fieldRepository.GetAll();

            if(request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Xóa thất bại.</exception>
        public async Task<string> Delete(String id)
        {
            try
            {
                await fieldRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch(Exception)
            {
                throw new Exception("Xóa thất bại.");
            }
        }

        /// <summary>
        /// Adds the field to group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy nhóm lĩnh vực.</exception>
        public async  Task<FieldGroupViewModel> AddFieldToGroup(AddFieldToGroupRequest request )
        {

            var group = await fieldGroupRepository.GetByIdAsync(ObjectId.Parse(request.GroupId));
            if (group == null)
                throw new Exception("Không tìm thấy nhóm lĩnh vực. ");

            foreach (var fieldId in request.FieldIds)
            {
                var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
                if (field != null)
                    group.FieldId.Add(fieldId);
            }
            await fieldGroupRepository.UpdateAsync(group, group.Id);

            return mapper.Map<FieldGroupViewModel>(group);
        }

        /// <summary>
        /// Add field group
        /// </summary>
        /// <param name="fieldGroup"></param>
        /// <returns></returns>
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

        public async Task<IEnumerable<Field>> GetFieldByGroupId(string groupId)
        {
            var group = await fieldGroupRepository.GetByIdAsync(ObjectId.Parse(groupId));
            if (group == null)
                throw new Exception("Không tìm thấy nhóm lĩnh vực. ");
            var groupViewModel = mapper.Map<FieldGroupViewModel>(group);

            return groupViewModel.Fields;

        }
    }
}

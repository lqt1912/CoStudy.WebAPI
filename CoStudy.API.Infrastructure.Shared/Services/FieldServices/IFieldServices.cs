using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Field Service Interface
    /// </summary>
    public interface IFieldServices
    {
        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<Field> AddField(Field entity);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<Field> GetAll(BaseGetAllRequest request);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> Delete(String id);

        Task<IEnumerable<Field>> GetFieldByGroupId(string groupId);

        /// <summary>
        /// Adds the field to group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<FieldGroupViewModel> AddFieldToGroup(AddFieldToGroupRequest request);

        /// <summary>
        /// Add FieldGroup
        /// </summary>
        /// <param name="fieldGroup"></param>
        /// <returns></returns>
        Task<FieldGroupViewModel> AddFieldGroup(FieldGroup fieldGroup);

    }
}

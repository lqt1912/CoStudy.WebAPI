﻿using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}

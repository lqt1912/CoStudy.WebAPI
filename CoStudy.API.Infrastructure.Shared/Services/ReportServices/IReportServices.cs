using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Report Services Interface
    /// </summary>
    public interface IReportServices
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<Report> Add(Report entity);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<Report> GetAll(BaseGetAllRequest request);

        /// <summary>
        /// Appvores the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        Task<IEnumerable<Report>> Appvore(IEnumerable<string> ids);
    }
}

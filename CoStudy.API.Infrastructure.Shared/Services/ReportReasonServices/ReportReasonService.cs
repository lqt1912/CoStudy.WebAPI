using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Report reason service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IReportReasonService" />
    public class ReportReasonService :IReportReasonService
    {
        /// <summary>
        /// The report reason repository
        /// </summary>
        IReportReasonRepository reportReasonRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportReasonService"/> class.
        /// </summary>
        /// <param name="reportReasonRepository">The report reason repository.</param>
        public ReportReasonService(IReportReasonRepository reportReasonRepository)
        {
            this.reportReasonRepository = reportReasonRepository;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<ReportReason> Add(ReportReason entity)
        {
            var data = new ReportReason()
            {
                Detail = entity.Detail
            };
            await reportReasonRepository.AddAsync(entity);
            return data;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<ReportReason> GetAll(BaseGetAllRequest request)
        {
            var data = reportReasonRepository.GetAll();
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
        /// <exception cref="Exception">Xóa thất bại</exception>
        public async Task<string> Delete(string id)
        {
            try
            {
                await reportReasonRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch (Exception)
            {
                throw new Exception("Xóa thất bại");
            }
        }
    }
}

using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
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
    /// The Report Service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IReportServices" />
    public class ReportServices : IReportServices
    {
        /// <summary>
        /// The report repository
        /// </summary>
        IReportRepository reportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportServices"/> class.
        /// </summary>
        /// <param name="reportRepository">The report repository.</param>
        public ReportServices(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Report> Add(Report entity)
        {
            var data = new Report()
            {
                CreatedDate = DateTime.Now,
                AuthorId = entity.AuthorId,
                IsApprove = false,
                ModifiedDate = DateTime.Now,
                Reason = entity.Reason,
                ObjectId = entity.ObjectId,
            };
            await reportRepository.AddAsync(data);
            return data;
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Report> GetAll(BaseGetAllRequest request)
        {
            var data = reportRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }


        /// <summary>
        /// Appvores the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Report>> Appvore(IEnumerable<string> ids)
        {
            var dataToApprove = new List<Report>();
            foreach (var item in ids)
            {
                var rp = await reportRepository.GetByIdAsync(ObjectId.Parse(item));
                if (rp != null)
                    dataToApprove.Add(rp);
            }

            foreach (var item in dataToApprove)
            {
                item.IsApprove = true;
                item.ModifiedDate = DateTime.Now;
                item.ApprovedBy = "Admin";
                await reportRepository.UpdateAsync(item, item.Id);
            }
            return dataToApprove;
        }

    }
}

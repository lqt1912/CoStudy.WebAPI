using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface IReportReasonService
    {
             Task<ReportReasonViewModel> Add(ReportReason entity);

             IEnumerable<ReportReasonViewModel> GetAll(BaseGetAllRequest request);
             Task<string> Delete(string id);
    }
}

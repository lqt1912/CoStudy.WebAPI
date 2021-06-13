using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface IReportServices
    {
             Task<ReportViewModel> Add(Report entity);

             IEnumerable<ReportViewModel> GetAll(BaseGetAllRequest request);

             Task<IEnumerable<ReportViewModel>> Approve(IEnumerable<string> ids);

             Task<ReportViewModel> PostReport(CreatePostReportRequest request);

             Task<ReportViewModel> CommentReport(CreateCommentReportRequest request);

             Task<ReportViewModel> ReplyReport(CreateReplyReportRequest request);

             Task<ReportViewModel> GetReportById(string id);
    }
}

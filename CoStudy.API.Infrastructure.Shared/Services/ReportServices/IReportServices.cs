using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
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
        Task<ReportViewModel> Add(Report entity);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<ReportViewModel> GetAll(BaseGetAllRequest request);

        /// <summary>
        /// Approves the specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        Task<IEnumerable<ReportViewModel>> Approve(IEnumerable<string> ids);

        /// <summary>
        /// Posts the report.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ReportViewModel> PostReport(CreatePostReportRequest request);

        /// <summary>
        /// Report the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ReportViewModel> CommentReport(CreateCommentReportRequest request);

        /// <summary>
        /// Replies the report.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ReportViewModel> ReplyReport(CreateReplyReportRequest request);
    }
}

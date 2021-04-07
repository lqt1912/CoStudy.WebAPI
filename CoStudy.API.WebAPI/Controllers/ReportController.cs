using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.ReportRequest;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class ReportConrtoller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        /// <summary>
        /// The report services
        /// </summary>
        IReportServices reportServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="reportServices">The report services.</param>
        public ReportController(IReportServices reportServices)
        {
            this.reportServices = reportServices;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(Report entity)
        {
            var data = await reportServices.Add(entity);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Reports the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("report-post")]
        public async Task<IActionResult> ReportPost(CreatePostReportRequest request)
        {
            var data = await reportServices.PostReport(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Reports the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("report-comment")]
        public async Task<IActionResult> ReportComment(CreateCommentReportRequest request)
        {
            var data = await reportServices.CommentReport(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Reports the reply.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("report-reply")]
        public async Task<IActionResult> ReportReply(CreateReplyReportRequest request)
        {
            var data = await reportServices.ReplyReport(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Approves the report.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [HttpPost("approve-report")]
        public async Task<IActionResult> ApproveReport(IEnumerable<string> ids)
        {
            var data = await reportServices.Approve(ids);
            return Ok(new ApiOkResponse(data));
        }
    }
}

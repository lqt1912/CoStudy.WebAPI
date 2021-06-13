using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IReportServices reportServices;

        public ReportController(IReportServices reportServices)
        {
            this.reportServices = reportServices;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Report entity)
        {
            var data = await reportServices.Add(entity);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("report-post")]
        public async Task<IActionResult> ReportPost(CreatePostReportRequest request)
        {
            var data = await reportServices.PostReport(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("report-comment")]
        public async Task<IActionResult> ReportComment(CreateCommentReportRequest request)
        {
            var data = await reportServices.CommentReport(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("report-reply")]
        public async Task<IActionResult> ReportReply(CreateReplyReportRequest request)
        {
            var data = await reportServices.ReplyReport(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("approve-report")]
        public async Task<IActionResult> ApproveReport(IEnumerable<string> ids)
        {
            var data = await reportServices.Approve(ids);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("all")]
        public IActionResult GetAll(BaseGetAllRequest request)
        {

            var data = reportServices.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            var data = await reportServices.GetReportById((id));
            return Ok(new ApiOkResponse(data));
        }
    }
}

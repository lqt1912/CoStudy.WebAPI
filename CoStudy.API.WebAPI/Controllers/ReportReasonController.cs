using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportReasonController : ControllerBase
    {
        IReportReasonService reportReasonService;

        public ReportReasonController(IReportReasonService reportReasonService)
        {
            this.reportReasonService = reportReasonService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReportReason entity)
        {
            var data = await reportReasonService.Add(entity);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] BaseGetAllRequest request)
        {
            var data = reportReasonService.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await reportReasonService.Delete(id);
            return Ok(new ApiOkResponse(data));
        }
    }
}
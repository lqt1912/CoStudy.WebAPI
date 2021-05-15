using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class ReportReasonController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class ReportReasonController : ControllerBase
    {
        /// <summary>
        /// The report reason service
        /// </summary>
        IReportReasonService reportReasonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportReasonController"/> class.
        /// </summary>
        /// <param name="reportReasonService">The report reason service.</param>
        public ReportReasonController(IReportReasonService reportReasonService)
        {
            this.reportReasonService = reportReasonService;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(ReportReason entity)
        {
            var data = await reportReasonService.Add(entity);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll([FromQuery] BaseGetAllRequest request)
        {
            var data = reportReasonService.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await reportReasonService.Delete(id);
            return Ok(new ApiOkResponse(data));
        }

    }
}

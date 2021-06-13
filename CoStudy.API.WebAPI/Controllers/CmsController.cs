using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Role.Admin)]
    public class CmsController : ControllerBase
    {
        ICmsServices cmsServices;

        public CmsController(ICmsServices cmsServices)
        {
            this.cmsServices = cmsServices;
        }

        [HttpPost("user/paged")]
        public IActionResult GetUserPaged(TableRequest request)
        {
            var data = cmsServices.GetUserPaged(request);
            return Ok(data);
        }

        [HttpPost("post/paged")]
        public IActionResult GetPostPaged(TableRequest request)
        {
            var data = cmsServices.GetPostPaged(request);
            return Ok(data);
        }

        [HttpPost("comment/paged")]
        public IActionResult GetCommentPaged(TableRequest request)
        {
            var data = cmsServices.GetCommentPaged(request);
            return Ok(data);
        }

        [HttpPost("reply-comment/paged")]
        public IActionResult GetReplyCommentPaged(TableRequest request)
        {
            var data = cmsServices.GetReplyCommentPaged(request);
            return Ok(data);
        }

        [HttpPost("report/paged")]
        public IActionResult GetReportPaged(TableRequest request)
        {
            var data = cmsServices.GetReportPaged(request);
            return Ok(data);
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var data = await cmsServices.GetByEmail(email);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("comment/{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var data = await cmsServices.GetCommentById(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("reply-comment/{id}")]
        public async Task<IActionResult> GetReplyCommentById(string id)
        {
            var data = await cmsServices.GetReplyCommentById(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            var data = await cmsServices.GetPostById(id);
            return Ok(new ApiOkResponse(data));
        }
    }
}

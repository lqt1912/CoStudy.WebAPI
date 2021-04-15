using CoStudy.API.Infrastructure.Shared.Paging;
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
    /// Class CmsControllers
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class CmsController : ControllerBase
    {
        /// <summary>
        /// The CMS services
        /// </summary>
        ICmsServices cmsServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsController"/> class.
        /// </summary>
        /// <param name="cmsServices">The CMS services.</param>
        public CmsController(ICmsServices cmsServices)
        {
            this.cmsServices = cmsServices;
        }

        /// <summary>
        /// Gets the user paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("user/paged")]
        public IActionResult GetUserPaged(TableRequest request)
        {
            var data = cmsServices.GetUserPaged(request);
            return Ok(data); 
        }

        /// <summary>
        /// Gets the post paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost("post/paged")]
        public IActionResult GetPostPaged(TableRequest request)
        {
            var data = cmsServices.GetPostPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var data = await cmsServices.GetByEmail(email);
            return Ok(new ApiOkResponse(data));
        }
    }
}

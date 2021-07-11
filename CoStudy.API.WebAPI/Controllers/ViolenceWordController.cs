using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Shared.Models.Request;
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
    [Route("api/[controller]")]
    [ApiController]
  
    public class ViolenceWordController : ControllerBase
    {
        private readonly IViolenceWordService violenceWordService;

        public ViolenceWordController(IViolenceWordService violenceWordService)
        {
            this.violenceWordService = violenceWordService;
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string value)
        {
            var data = await violenceWordService.Add(value);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        public IActionResult Get([FromQuery] BaseGetAllRequest request)
        {
            var data = violenceWordService.GetAll(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await violenceWordService.Delete(id);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize(Role.Admin)]
        [HttpPost("paged")]
        public IActionResult Paged(TableRequest request)
        {
            var data = violenceWordService.GetViolenceWordPaged(request);
            return Ok(data);
        }
    }
}

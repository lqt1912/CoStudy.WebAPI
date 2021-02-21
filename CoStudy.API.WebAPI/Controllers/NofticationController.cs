using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NofticationController : ControllerBase
    {
        INofticationService nofticationService;

        public NofticationController(INofticationService nofticationService)
        {
            this.nofticationService = nofticationService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNoftication(AddNofticationRequest request)
        {
            Infrastructure.Shared.Models.Response.NofticationResponse.AddNofticationResponse data = await nofticationService.AddNoftication(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentUserNoftication(int? skip, int? count)
        {
            System.Collections.Generic.IEnumerable<Domain.Entities.Application.Noftication> data = await nofticationService.GetCurrentUserNoftication(skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string data = await nofticationService.DeleteNotification(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            string data = await nofticationService.MarkAsRead(id);
            return Ok(new ApiOkResponse(data));
        }
    }
}

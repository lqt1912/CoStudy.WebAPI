using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var data = await nofticationService.AddNoftication(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrentUserNoftication()
        {
            var data = nofticationService.GetCurrentUserNoftication();
            return Ok(new ApiOkResponse(data));
        }
    }
}

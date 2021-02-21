using CoStudy.API.Application.FCM;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FcmController : ControllerBase
    {
        IFcmRepository fcmRepository;

        public FcmController(IFcmRepository fcmRepository)
        {
            this.fcmRepository = fcmRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddFcm(string userId, string token)
        {
            Domain.Entities.Application.FcmInfo data = await fcmRepository.AddFcmInfo(userId, token);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> RevokeFcm(string userId, string token)
        {
            Domain.Entities.Application.FcmInfo data = await fcmRepository.RevokeFcmInfo(userId, token);
            return Ok(new ApiOkResponse(data));
        }

    }
}

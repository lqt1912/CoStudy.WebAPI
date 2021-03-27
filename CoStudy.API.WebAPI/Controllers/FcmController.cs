using CoStudy.API.Application.FCM;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPost]
        [Route("add-to-group")]
        public async Task<IActionResult> AddToGroup(AddUserToGroupRequest request)
        {
            await fcmRepository.AddToGroup(request);
            return Ok(new ApiOkResponse("Add Thành công"));
        }


        [HttpPost]
        [Route("remove-from-group")]
        public async Task<IActionResult> RemoveFromGroup(RemoveFromGroupRequest request)
        {
            await fcmRepository.RemoveFromGroup(request);
            return Ok(new ApiOkResponse("Xóa thành công; "));
        }
    }
}

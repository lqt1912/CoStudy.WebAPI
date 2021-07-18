using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpPost("add-type")]
        public async Task<IActionResult> AddNotificationType(NotificationType request)
        {
            var data = await nofticationService.AddNotificationType(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNoftication(AddNofticationRequest request)
        {
            NotificationViewModel data = await nofticationService.AddNoftication(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentUserNoftication([FromQuery] GetAllNotificationRequest request)
        {
            var data =await  nofticationService.GetCurrentUserNotificationList(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string data = await nofticationService.DeleteCurrentNotification(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            string data = await nofticationService.MarkNotificaionsAsRead(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("delete-all")]
        public async Task<IActionResult> DeleteAll()
        {
            var data = await nofticationService.DeleteNotification();
            return Ok(new ApiOkResponse(data));
        }
    }
}

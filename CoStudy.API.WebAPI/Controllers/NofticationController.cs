using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.Domain.Entities.Application;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class NotificationController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NofticationController : ControllerBase
    {
        /// <summary>
        /// The noftication service
        /// </summary>
        INofticationService nofticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NofticationController"/> class.
        /// </summary>
        /// <param name="nofticationService">The noftication service.</param>
        public NofticationController(INofticationService nofticationService)
        {
            this.nofticationService = nofticationService;
        }


        /// <summary>
        /// Adds the type of the notification.
        /// </summary>
        /// <param name="requst">The requst.</param>
        /// <returns></returns>
        [HttpPost("add-type")]
        public async Task<IActionResult> AddNotificationType(NotificationType request)
        {
            var data = await nofticationService.AddNotificationType(request);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Adds the noftication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNoftication(AddNofticationRequest request)
        {
          NotificationViewModel data = await nofticationService.AddNoftication(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the current user noftication.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentUserNoftication()
        {
            IEnumerable<NotificationViewModel> data = await nofticationService.GetCurrentUserNotificationList();
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string data = await nofticationService.DeleteCurrentNotification(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Marks as read.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            string data = await nofticationService.MarkNotificaionsAsRead(id);
            return Ok(new ApiOkResponse(data));
        }
    }
}

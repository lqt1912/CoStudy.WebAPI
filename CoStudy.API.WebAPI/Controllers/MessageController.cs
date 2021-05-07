using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// The Message Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        /// <summary>
        /// The message service
        /// </summary>
        IMessageService messageService;
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="messageService">The message service.</param>
        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }


        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("message/add")]
        public async Task<IActionResult> AddMessage(AddMessageRequest request)
        {
            MessageViewModel data = await messageService.AddMessage(request);

            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the message by conversation identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("message/get/conversation")]
        public async Task<IActionResult> GetMessageByConversationId([FromQuery] GetMessageByConversationIdRequest request)
        {
            IEnumerable<MessageViewModel> data = await messageService.GetMessageByConversationId(request);
            return Ok(new ApiOkResponse(data));
        }



        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("message/{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            string data = await messageService.DeleteMessage(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> EditMessage(UpdateMessageRequest request)
        {
            MessageViewModel data = await messageService.EditMessage(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            return Ok(messageService.GetAll());
        }

    }
}

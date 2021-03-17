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
        /// Adds the conversation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("conversation/add")]
        public async Task<IActionResult> AddConversation(AddConversationRequest request)
        {
            ConversationViewModel data = await messageService.AddConversation(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the current conversation list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("conversation/current")]
        public IActionResult GetCurrentConversationList()
        {
            GetConversationByUserIdResponse data = messageService.GetConversationByUserId();
            return Ok(new ApiOkResponse(data));
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
        /// Deletes the conversation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("conversation/{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            string data = await messageService.DeleteConversation(id);
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


        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            IEnumerable<MessageViewModel> data = await messageService.AddMember(request);
            return Ok(new ApiOkResponse(data));
        }

    }
}

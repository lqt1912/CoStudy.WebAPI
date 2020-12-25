using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.WebAPI.Middlewares;
using CoStudy.API.WebAPI.SignalR.DI.Message;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        IMessageService messageService;
        IMessageHub messageHub;
        public MessageController(IMessageService messageService, IMessageHub messagetHub)
        {
            this.messageService = messageService;
            this.messageHub = messagetHub;
        }

        [HttpPost]
        [Route("conversation/add")]
        public async Task<IActionResult> AddConversation(AddConversationRequest request)
        {
            var data = await messageService.AddConversation(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("conversation/current")]
        public IActionResult GetCurrentConversationList()
        {
            var data = messageService.GetConversationByUserId();
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("message/add")]
        public async Task<IActionResult> AddMessage([FromForm] AddMessageRequest request)
        {
            var data = await messageService.AddMessage(request);
            await messageHub.SendGlobal(data);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("message/get/conversation/{id}/limit/{limit}")]
        public IActionResult GetMessageByConversationId(string id, int limit)
        {
            var data = messageService.GetMessageByConversationId(id, limit);

            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            return Ok(messageService.GetAll());
        }
    }
}

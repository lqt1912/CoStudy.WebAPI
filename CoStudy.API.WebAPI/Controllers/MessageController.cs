using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        IMessageService messageService;
        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
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
        public async Task<IActionResult> AddMessage(AddMessageRequest request)
        {
            var data = await messageService.AddMessage(request);

            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("message/get/conversation/{id}/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetMessageByConversationId(string id, int skip, int count)
        {
            var data = await messageService.GetMessageByConversationId(id, skip, count);
            return Ok(new ApiOkResponse(data));
        }


        [HttpDelete]
        [Route("conversation/{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            var data = await messageService.DeleteConversation(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("message/{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            var data = await messageService.DeleteMessage(id);
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

using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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


             [Authorize]
        [HttpPost]
        [Route("message/add")]
        public async Task<IActionResult> AddMessage(AddMessageRequest request)
        {
            MessageViewModel data = await messageService.AddMessage(request);

            return Ok(new ApiOkResponse(data));
        }

             [HttpGet]
        [Route("message/get/conversation")]
        public async Task<IActionResult> GetMessageByConversationId([FromQuery] GetMessageByConversationIdRequest request)
        {
            IEnumerable<MessageViewModel> data = await messageService.GetMessageByConversationId(request);
            return Ok(new ApiOkResponse(data));
        }



             [HttpDelete]
        [Route("message/{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            string data = await messageService.DeleteMessage(id);
            return Ok(new ApiOkResponse(data));
        }

             [HttpPut]
        [Route("update")]
        public async Task<IActionResult> EditMessage(UpdateMessageRequest request)
        {
            MessageViewModel data = await messageService.EditMessage(request);
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

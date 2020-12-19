using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[Authorize]
        [HttpPost]
        [Route("message/add")]
        public async Task<IActionResult> AddMessage([FromForm]AddMessageRequest request)
        {
            var data = await messageService.AddMessage(request);
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

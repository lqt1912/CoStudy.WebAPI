using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
        [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

           IConversationService conversationService;

            public ConversationController(IConversationService conversationService)
        {
            this.conversationService = conversationService;
        }

             [HttpPost]
        public async Task<IActionResult> AddConversation(AddConversationRequest request)
        {
            ConversationViewModel data = await conversationService.AddConversation(request);
            return Ok(new ApiOkResponse(data));
        }


            [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrentConversationList()
        {
            GetConversationByUserIdResponse data = await conversationService.GetConversationByUserId();
            return Ok(new ApiOkResponse(data));
        }


             [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            string data = await conversationService.DeleteConversation(id);
            return Ok(new ApiOkResponse(data));
        }

             [HttpPut]
        [Route("member")]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            IEnumerable<MessageViewModel> data = await conversationService.AddMember(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

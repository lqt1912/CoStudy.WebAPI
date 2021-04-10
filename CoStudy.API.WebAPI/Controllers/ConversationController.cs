using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class ConversationController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

        /// <summary>
        /// The conversation service
        /// </summary>
        IConversationService conversationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationController"/> class.
        /// </summary>
        /// <param name="conversationService">The conversation service.</param>
        public ConversationController(IConversationService conversationService)
        {
            this.conversationService = conversationService;
        }

        /// <summary>
        /// Adds the conversation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddConversation(AddConversationRequest request)
        {
            ConversationViewModel data = await conversationService.AddConversation(request);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Gets the current conversation list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrentConversationList()
        {
            GetConversationByUserIdResponse data = conversationService.GetConversationByUserId();
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Deletes the conversation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteConversation(string id)
        {
            string data = await conversationService.DeleteConversation(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("member")]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            IEnumerable<MessageViewModel> data = await conversationService.AddMember(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

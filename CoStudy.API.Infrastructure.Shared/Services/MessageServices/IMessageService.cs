using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    public interface IMessageService
    {
        Task<AddConversationResponse> AddConversation(AddConversationRequest request);
        Task<AddMessageResponse> AddMessage(AddMessageRequest request);
        GetMessageByConversationIdResponse GetMessageByConversationId(string conversationId, int limit);
        GetConversationByUserIdResponse GetConversationByUserId();
        List<Message> GetAll();
    }
}

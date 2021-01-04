using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    public interface IMessageService
    {
        Task<AddConversationResponse> AddConversation(AddConversationRequest request);
        Task<AddMessageResponse> AddMessage(AddMessageRequest request);
        Task<GetMessageByConversationIdResponse> GetMessageByConversationId(string conversationId, int skip, int count);
        GetConversationByUserIdResponse GetConversationByUserId();
        List<Message> GetAll();
    }
}

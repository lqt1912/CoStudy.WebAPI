using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR.DI.Message
{
    public interface IMessageHub
    {
        Task SendGlobal(AddMessageResponse addMessageResponse);
        Task SendConversation(string conversationId, AddMessageResponse addMessageResponse);

    }
}

using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR.DI.Message
{
    public class MessageHub : IMessageHub
    {
        private IHubContext<SignalRHub<AddMessageResponse>, IHubClient<AddMessageResponse>> _signalrHub;

        public MessageHub(IHubContext<SignalRHub<AddMessageResponse>, IHubClient<AddMessageResponse>> signalrHub)
        {
            _signalrHub = signalrHub;
        }

        public async Task SendGlobal(AddMessageResponse addMessageResponse)
        {
            await _signalrHub.Clients.All.SendNofti(addMessageResponse);
        }
    }
}

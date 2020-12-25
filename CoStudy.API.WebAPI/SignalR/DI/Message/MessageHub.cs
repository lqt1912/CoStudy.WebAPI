using CoStudy.API.Application.Repositories;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR.DI.Message
{
    public class MessageHub : IMessageHub
    {
        private IHubContext<SignalRHub<AddMessageResponse>, IHubClient<AddMessageResponse>> _signalrHub;
        IClientGroupRepository clientGroupRepository;
        IClientConnectionsRepository clientConnectionsRepository;
        IConversationRepository conversationRepository;
        IUserRepository userRepository;

        public MessageHub(
            IHubContext<SignalRHub<AddMessageResponse>, IHubClient<AddMessageResponse>> signalrHub, 
            IClientConnectionsRepository clientConnectionsRepository, 
            IClientGroupRepository clientGroupRepository, 
            IConversationRepository conversationRepository, 
            IUserRepository userRepository)
        {
            _signalrHub = signalrHub;
            this.clientConnectionsRepository = clientConnectionsRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.conversationRepository = conversationRepository;
            this.userRepository = userRepository;
        }

        public async Task SendConversation(string conversationId, AddMessageResponse addMessageResponse)
        {
            var currentConversation =await conversationRepository.GetByIdAsync(ObjectId.Parse(conversationId));
            if(currentConversation!=null)
            {
                var participants = currentConversation.Participants;
                foreach (var participant in participants)
                {
                    var currentUser = await userRepository.GetByIdAsync(ObjectId.Parse(participant));
                    if(currentUser!=null)
                    {
                        var clientConnections = await clientConnectionsRepository.GetByIdAsync(ObjectId.Parse(currentUser.ClientConnectionsId));
                       
                        await _signalrHub.Clients.Clients(clientConnections.ClientConnection).SendNofti(addMessageResponse);
                    }    
                }
            }
            throw new System.NotImplementedException();
        }

        public async Task SendGlobal(AddMessageResponse addMessageResponse)
        {
            await _signalrHub.Clients.All.SendNofti(addMessageResponse);
        }
    }
}

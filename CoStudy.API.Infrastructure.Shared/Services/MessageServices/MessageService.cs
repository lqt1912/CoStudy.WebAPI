using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        IMessageRepository messageRepository;
        IConversationRepository conversationRepository;
        IUserRepository userRepository;
        IHttpContextAccessor httpContextAccessor;
        public IClientConnectionsRepository clientConnectionsRepository { get; }
        public IClientGroupRepository clientGroupRepository { get; }

        public MessageService(IMessageRepository messageRepository, IConversationRepository conversationRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IClientConnectionsRepository clientConnectionsRepository, IClientGroupRepository clientGroupRepository)
        {
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.clientConnectionsRepository = clientConnectionsRepository;
            this.clientGroupRepository = clientGroupRepository;
        }



        //IHubContext<BaseHub<Message>, IBaseHub<Message>> messageHub;
        //public MessageService(IMessageRepository messageRepository,
        //    IConversationRepository conversationRepository,
        //    IUserRepository userRepository,
        //    IHttpContextAccessor httpContextAccessor, IHubContext<BaseHub<Message>, IBaseHub<Message>> messageHub, IClientConnectionsRepository clientConnectionsRepository, IClientGroupRepository clientGroupRepository)
        //{
        //    this.messageRepository = messageRepository;
        //    this.conversationRepository = conversationRepository;
        //    this.userRepository = userRepository;
        //    this.httpContextAccessor = httpContextAccessor;
        //    this.messageHub = messageHub;
        //    this.clientConnectionsRepository = clientConnectionsRepository;
        //    this.clientGroupRepository = clientGroupRepository;
        //}

        public async Task<AddConversationResponse> AddConversation(AddConversationRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            request.Participants.Add(currentUser.Id.ToString());
            var conversation = MessageAdapter.FromRequest(request);

            await conversationRepository.AddAsync(conversation);
            return MessageAdapter.ToResponse(conversation);
        }


        public async Task<AddMessageResponse> AddMessage(AddMessageRequest request)
        {
            var message = MessageAdapter.FromRequest(request, httpContextAccessor, userRepository);

            await messageRepository.AddAsync(message);

            try
            {
                var currentConversation = await conversationRepository.GetByIdAsync(ObjectId.Parse(request.ConversationId));
                var clientGroup = await clientGroupRepository.GetByIdAsync(ObjectId.Parse(currentConversation.ClientGroupId));

                foreach (var clientConnectionsId in clientGroup.ConnectionGroupIds)
                {
                    var clientConnections = await clientConnectionsRepository.GetByIdAsync(ObjectId.Parse(clientConnectionsId));
                    //await messageHub.Clients.Clients(clientConnections.ClientConnection).BroadCast(message);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
          //  await messageHub.Clients.All.BroadCast(message);
            return MessageAdapter.ToResponse(message);
        }



        public GetMessageByConversationIdResponse GetMessageByConversationId(string conversationId, int limit)
        {
            var messages = messageRepository.GetAll().Where(x => x.ConversationId == conversationId).Take(limit).ToList();
            return new GetMessageByConversationIdResponse()
            {
                Id = conversationId,
                Messages = messages
            };
        }
        public GetConversationByUserIdResponse GetConversationByUserId()
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            if (currentUser == null)
                throw new Exception("User not found");
            var result = new List<Tuple<Conversation, Message>>();
            var conversations = conversationRepository.GetAll().Where(x => x.Participants.Contains(currentUser.Id.ToString()));
            foreach (var conversation in conversations)
            {
                var recentMessage = messageRepository.GetAll().Where(x => x.ConversationId == conversation.Id.ToString()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (recentMessage == null)
                    recentMessage = new Message();
                var item = new Tuple<Conversation, Message>(conversation, recentMessage);
                result.Add(item);
            }
            return new GetConversationByUserIdResponse()
            {
                Conversations = result
            };
        }

        public  List<Message> GetAll()
        {
            return messageRepository.GetAll().ToList();
        }
    }
}


using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        IMessageRepository messageRepository;
        IConversationRepository conversationRepository;
        IUserRepository userRepository;
        IHttpContextAccessor httpContextAccessor;
        IFcmRepository fcmRepository;

        public IClientGroupRepository clientGroupRepository { get; }

        public MessageService(IMessageRepository messageRepository, 
            IConversationRepository conversationRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor, 
            IClientGroupRepository clientGroupRepository,
            IFcmRepository fcmRepository)
        {
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.clientGroupRepository = clientGroupRepository;
            this.fcmRepository = fcmRepository;
        }

        public async Task<AddConversationResponse> AddConversation(AddConversationRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            request.Participants.Add(currentUser.Id.ToString());

            var existConversation = new Conversation() { Participants = new List<string>() };
                
            foreach (var conver in conversationRepository.GetAll())
            {
                if (Feature.IsEqual(conver.Participants, request.Participants) == true)
                {
                    existConversation = conver;
                    break;
                }
            }

            if (existConversation.Participants.Count !=0)
                return MessageAdapter.ToResponse(existConversation);

            var conversation = MessageAdapter.FromRequest(request);

            await conversationRepository.AddAsync(conversation);

            var clientGroup = new ClientGroup()
            {
                UserIds = request.Participants,
                Name = conversation.Id.ToString(),
            };
            await clientGroupRepository.AddAsync(clientGroup);
            return MessageAdapter.ToResponse(conversation);
        }

        public async Task<AddMessageResponse> AddMessage(AddMessageRequest request)
        {
            var message = MessageAdapter.FromRequest(request, httpContextAccessor, userRepository);

            await messageRepository.AddAsync(message);

            await fcmRepository.SendMessage(request.ConversationId, message);

            return MessageAdapter.ToResponse(message);
        }

        public async Task<GetMessageByConversationIdResponse> GetMessageByConversationId(string conversationId, int skip, int count)
        {
            try
            {
                var finder = Builders<Message>.Filter;
                var filter = finder.Eq("conversation_id", conversationId) & finder.Eq("status", ItemStatus.Active);
                var messages = (await messageRepository.FindListAsync(filter)).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(count);
                return new GetMessageByConversationIdResponse()
                {
                    Id = conversationId,
                    Messages = messages
                };

            }
            catch (Exception)
            {
                throw new Exception("Đã có lỗi xảy ra.");
            }
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

        public List<Message> GetAll()
        {
            return messageRepository.GetAll().ToList();
        }

        public async Task<string> DeleteConversation(string id)
        {
            var exist = await conversationRepository.GetByIdAsync(ObjectId.Parse(id));
            if (exist != null)
            {
                //delete conversation
                await conversationRepository.DeleteAsync(ObjectId.Parse(id));

                //Delete notification group
                var finder = Builders<ClientGroup>.Filter.Eq("name", id);
                var clientGroup = await clientGroupRepository.FindAsync(finder);
                if (clientGroup != null)
                    await clientGroupRepository.DeleteAsync(clientGroup.Id);

                return "Xóa cuộc trò chuyện thành công";
            }
            else throw new Exception("Đã có lỗi xảy ra");
        }

        public async Task<string> DeleteMessage(string id)
        {
            var existMessage = await messageRepository.GetByIdAsync(ObjectId.Parse(id));
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            if (existMessage != null && existMessage.SenderId == currentUser.OId)
            {
                existMessage.Status = ItemStatus.Deleted;
                await messageRepository.UpdateAsync(existMessage, existMessage.Id);
                return "Xóa tin nhắn thành công";
            }
            else throw new Exception("Đã có lỗi xảy ra");
        }
    }
}


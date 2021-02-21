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

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            request.Participants.Add(currentUser.Id.ToString());

            Conversation existConversation = new Conversation() { Participants = new List<string>() };

            foreach (Conversation conver in conversationRepository.GetAll())
            {
                if (Feature.IsEqual(conver.Participants, request.Participants) == true)
                {
                    existConversation = conver;
                    break;
                }
            }

            if (existConversation.Participants.Count != 0)
                return MessageAdapter.ToResponse(existConversation);

            Conversation conversation = MessageAdapter.FromRequest(request);

            await conversationRepository.AddAsync(conversation);

            ClientGroup clientGroup = new ClientGroup()
            {
                UserIds = request.Participants,
                Name = conversation.Id.ToString(),
            };
            await clientGroupRepository.AddAsync(clientGroup);
            return MessageAdapter.ToResponse(conversation);
        }

        public async Task<AddMessageResponse> AddMessage(AddMessageRequest request)
        {
            Message message = MessageAdapter.FromRequest(request, httpContextAccessor, userRepository);

            await messageRepository.AddAsync(message);

            await fcmRepository.SendMessage(request.ConversationId, message);

            return MessageAdapter.ToResponse(message);
        }

        public async Task<GetMessageByConversationIdResponse> GetMessageByConversationId(string conversationId, int skip, int count)
        {
            try
            {
                FilterDefinitionBuilder<Message> finder = Builders<Message>.Filter;
                FilterDefinition<Message> filter = finder.Eq("conversation_id", conversationId) & finder.Eq("status", ItemStatus.Active);
                IEnumerable<Message> messages = (await messageRepository.FindListAsync(filter)).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(count);
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
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            if (currentUser == null)
                throw new Exception("User not found");
            List<Tuple<Conversation, Message>> result = new List<Tuple<Conversation, Message>>();

            IQueryable<Conversation> conversations = conversationRepository.GetAll().Where(x => x.Participants.Contains(currentUser.Id.ToString()));
            foreach (Conversation conversation in conversations)
            {
                Message recentMessage = messageRepository.GetAll().Where(x => x.ConversationId == conversation.Id.ToString()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (recentMessage == null)
                    recentMessage = new Message();
                Tuple<Conversation, Message> item = new Tuple<Conversation, Message>(conversation, recentMessage);
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
            Conversation exist = await conversationRepository.GetByIdAsync(ObjectId.Parse(id));
            if (exist != null)
            {
                //delete conversation
                await conversationRepository.DeleteAsync(ObjectId.Parse(id));

                //Delete notification group
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", id);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);
                if (clientGroup != null)
                    await clientGroupRepository.DeleteAsync(clientGroup.Id);

                return "Xóa cuộc trò chuyện thành công";
            }
            else throw new Exception("Đã có lỗi xảy ra");
        }

        public async Task<string> DeleteMessage(string id)
        {
            Message existMessage = await messageRepository.GetByIdAsync(ObjectId.Parse(id));
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            if (existMessage != null && existMessage.SenderId == currentUser.OId)
            {
                existMessage.Status = ItemStatus.Deleted;
                await messageRepository.UpdateAsync(existMessage, existMessage.Id);
                return "Xóa tin nhắn thành công";
            }
            else throw new Exception("Đã có lỗi xảy ra");
        }

        public async Task<Message> EditMessage(UpdateMessageRequest request)
        {
            Message message = await messageRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (message == null)
                throw new Exception("Tin nhắn không tìm thấy");
            message.MediaContent = request.Image;
            message.StringContent = request.Content;
            message.IsEdited = true;
            await messageRepository.UpdateAsync(message, message.Id);
            return message;
        }
    }
}


using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.MessageServices
{
    /// <summary>
    /// Class MessageService.
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.MessageServices.IMessageService" />
    public class MessageService : IMessageService
    {
        /// <summary>
        /// The message repository
        /// </summary>
        IMessageRepository messageRepository;
        /// <summary>
        /// The conversation repository
        /// </summary>
        IConversationRepository conversationRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// The FCM repository
        /// </summary>
        IFcmRepository fcmRepository;
        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;
        /// <summary>
        /// Gets the client group repository.
        /// </summary>
        /// <value>
        /// The client group repository.
        /// </value>
        public IClientGroupRepository clientGroupRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="conversationRepository">The conversation repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="fcmRepository">The FCM repository.</param>
        /// <param name="mapper">The mapper.</param>
        public MessageService(IMessageRepository messageRepository,
            IConversationRepository conversationRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IClientGroupRepository clientGroupRepository,
            IFcmRepository fcmRepository, IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.clientGroupRepository = clientGroupRepository;
            this.fcmRepository = fcmRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds the conversation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ConversationViewModel> AddConversation(AddConversationRequest request)
        {

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var firstmember = new ConversationMember()
            {
                DateJoin = DateTime.Now,
                JoinBy = currentUser.OId,
                MemberId = currentUser.OId,
                Nickname = $"{currentUser.FirstName} {currentUser.LastName}",
                Role = ConversationRole.Admin
            };


            request.Participants.Add(firstmember);

            Conversation existConversation = new Conversation() { Participants = new List<ConversationMember>() };

            foreach (Conversation conver in conversationRepository.GetAll())
            {
                if (Feature.IsEqual(conver.Participants, request.Participants) == true)
                {
                    existConversation = conver;
                    break;
                }
            }

            if (existConversation.Participants.Count != 0)
                return mapper.Map<ConversationViewModel>(existConversation);

            Conversation conversation = MessageAdapter.FromRequest(request);

            await conversationRepository.AddAsync(conversation);

            ClientGroup clientGroup = new ClientGroup()
            {
                UserIds = request.Participants.Select(x => x.MemberId).ToList(),
                Name = conversation.Id.ToString(),
            };
            await clientGroupRepository.AddAsync(clientGroup);
            return mapper.Map<ConversationViewModel>(conversation);
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<MessageViewModel> AddMessage(AddMessageRequest request)
        {
            Message message = MessageAdapter.FromRequest(request, httpContextAccessor, userRepository);

            await messageRepository.AddAsync(message);

            await fcmRepository.SendMessage(request.ConversationId, message);

            return mapper.Map<MessageViewModel>(message);
        }

        /// <summary>
        /// Gets the message by conversation identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Đã có lỗi xảy ra.</exception>
        public async Task<IEnumerable<MessageViewModel>> GetMessageByConversationId(GetMessageByConversationIdRequest request)
        {
            try
            {
                FilterDefinitionBuilder<Message> finder = Builders<Message>.Filter;
                FilterDefinition<Message> filter = finder.Eq("conversation_id", request.ConversationId) & finder.Eq("status", ItemStatus.Active);
                var messages = (await messageRepository.FindListAsync(filter)).OrderByDescending(x => x.CreatedDate).ToList();
                if (request.Skip.HasValue && request.Count.HasValue)
                    messages = messages.Skip(request.Skip.Value).Take(request.Count.Value).ToList();

                return mapper.Map<IEnumerable<MessageViewModel>>(messages);

            }
            catch (Exception)
            {
                throw new Exception("Đã có lỗi xảy ra.");
            }
        }

        /// <summary>
        /// Gets the conversation by user identifier.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">User not found</exception>
        public GetConversationByUserIdResponse GetConversationByUserId()
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            if (currentUser == null)
                throw new Exception("User not found");

            var listData = new List<ConversationData>();

            var conversations = new List<Conversation>();
            foreach (var item in conversationRepository.GetAll())
            {
                foreach (var i in item.Participants)
                {
                    if (i.MemberId == currentUser.OId)
                        conversations.Add(item);
                }
            }

            foreach (Conversation conversation in conversations)
            {
                IEnumerable<Message> recentMessage = messageRepository.GetAll().Where(x => x.ConversationId == conversation.Id.ToString()).OrderByDescending(x => x.CreatedDate).Take(5);
                if (recentMessage.Count() == 0)
                    recentMessage = new List<Message>();
                var messageViewModel = mapper.Map<IEnumerable<MessageViewModel>>(recentMessage);

                var conversationViewModel = mapper.Map<ConversationViewModel>(conversation);

                listData.Add(new ConversationData()
                {
                    Conversation = conversationViewModel,
                    Messages = messageViewModel
                });
            }
            return new GetConversationByUserIdResponse()
            {
                Conversations = listData
            };
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public List<Message> GetAll()
        {
            return messageRepository.GetAll().ToList();
        }

        /// <summary>
        /// Deletes the conversation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Đã có lỗi xảy ra</exception>
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

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Đã có lỗi xảy ra</exception>
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

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Tin nhắn không tìm thấy</exception>
        public async Task<MessageViewModel> EditMessage(UpdateMessageRequest request)
        {
            Message message = await messageRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (message == null)
                throw new Exception("Tin nhắn không tìm thấy");
            message.MediaContent = request.Image;
            message.StringContent = request.Content;
            message.IsEdited = true;
            await messageRepository.UpdateAsync(message, message.Id);
            return mapper.Map<MessageViewModel>(message);
        }


        public async Task<IEnumerable<MessageViewModel>> AddMember(AddMemberRequest request)
        {
            var conversation = await conversationRepository.GetByIdAsync(ObjectId.Parse(request.ConversationId));

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var currentMember = conversation.Participants.FirstOrDefault(x => x.MemberId == currentUser.OId);
            if (currentMember.Role != ConversationRole.Admin)
                throw new Exception("Bạn không có quyền thêm người dùng mới vào cuộc trò chuyện. ");

            var result = new List<Message>();

            foreach (var userId in request.UserIds)
            {
                if (conversation.Participants.FirstOrDefault(x => x.MemberId == userId) == null)
                {
                    var user = await userRepository.GetByIdAsync(ObjectId.Parse(userId));
                    var conversationMember = new ConversationMember()
                    {
                        MemberId = user.OId,
                        DateJoin = DateTime.Now,
                        JoinBy = currentUser.OId,
                        Nickname = $"{user.FirstName} {user.LastName}",
                        Role = ConversationRole.Admin
                    };
                    conversation.Participants.Add(conversationMember);
                    await conversationRepository.UpdateAsync(conversation, conversation.Id);

                    var message = new Message()
                    {
                        SenderId = currentUser.OId,
                        ConversationId = conversation.OId,
                        StringContent = $"{user.FirstName} {user.LastName} đã được thêm vào nhóm",
                        Status = ItemStatus.Active,
                        IsEdited = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    await messageRepository.AddAsync(message);
                    result.Add(message);
                }
            }

            return mapper.Map<IEnumerable<MessageViewModel>>(result);
        }

    }
}


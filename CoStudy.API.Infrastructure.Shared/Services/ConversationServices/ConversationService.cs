using AutoMapper;
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

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Class ConversationService
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IConversationService" />
    public class ConversationService : IConversationService
    {
        /// <summary>
        /// The conversation item type repository
        /// </summary>
        IConversationItemTypeRepository conversationItemTypeRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The conversation repository
        /// </summary>
        private IConversationRepository conversationRepository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The client group repository
        /// </summary>
        private IClientGroupRepository clientGroupRepository;

        /// <summary>
        /// The message repository
        /// </summary>
        private IMessageRepository messageRepository;

        /// <summary>
        /// The messag text repository
        /// </summary>
        private IMessageTextRepository messagTextRepository;

        /// <summary>
        /// The message image repository
        /// </summary>
        private IMessageImageRepository messageImageRepository;

        /// <summary>
        /// The message multi media repository
        /// </summary>
        private IMessageMultiMediaRepository messageMultiMediaRepository;

        /// <summary>
        /// The message post thumbnail repository
        /// </summary>
        private IMessagePostThumbnailRepository messagePostThumbnailRepository;

        /// <summary>
        /// The message conversation activity repository
        /// </summary>
        private IMessageConversationActivityRepository messageConversationActivityRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationService"/> class.
        /// </summary>
        /// <param name="conversationItemTypeRepository">The conversation item type repository.</param>
        public ConversationService(IConversationItemTypeRepository conversationItemTypeRepository,
            IMapper mapper, IConversationRepository conversationRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IClientGroupRepository clientGroupRepository,
            IMessageRepository messageRepository, 
            IMessageTextRepository messagTextRepository, 
            IMessageImageRepository messageImageRepository, 
            IMessageMultiMediaRepository messageMultiMediaRepository, 
            IMessagePostThumbnailRepository messagePostThumbnailRepository, 
            IMessageConversationActivityRepository messageConversationActivityRepository)
        {
            this.conversationItemTypeRepository = conversationItemTypeRepository;
            this.mapper = mapper;
            this.conversationRepository = conversationRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.messageRepository = messageRepository;
            this.messagTextRepository = messagTextRepository;
            this.messageImageRepository = messageImageRepository;
            this.messageMultiMediaRepository = messageMultiMediaRepository;
            this.messagePostThumbnailRepository = messagePostThumbnailRepository;
            this.messageConversationActivityRepository = messageConversationActivityRepository;
        }

        /// <summary>
        /// Adds the type of the conversation item.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<ConversationItemTypeViewModel> AddConversationItemType(ConversationItemType entity)
        {

            var data = new ConversationItemType()
            {
                Code = entity.Code,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Name = entity.Name
            };
            await conversationItemTypeRepository.AddAsync(data);
            return mapper.Map<ConversationItemTypeViewModel>( data);
        }

        /// <summary>
        /// Gets the item type by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public async Task<ConversationItemTypeViewModel> GetItemTypeByCode(string code)
        {
            var conversationItemTypeFilter = Builders<ConversationItemType>.Filter.Eq("code", code);
            var data = await conversationItemTypeRepository.FindAsync(conversationItemTypeFilter);
            return mapper.Map<ConversationItemTypeViewModel>(data);
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

            Conversation conversation = MessageAdapter.FromRequest(request, httpContextAccessor, userRepository);

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
        /// Gets the conversation by user identifier.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">User not found</exception>
        public async Task <GetConversationByUserIdResponse> GetConversationByUserId()
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
                var latestTextMessage = (await messagTextRepository.FindListAsync(Builders<MessageText>.Filter.Eq("conversation_id", conversation.OId))).OrderByDescending(x => x.CreatedDate).Take(1);
                var latestImageMessage = (await messageImageRepository.FindListAsync(Builders<MessageImage>.Filter.Eq("conversation_id", conversation.OId))).OrderByDescending(x => x.CreatedDate).Take(1);
                var latestPostThumbnailMessage = (await messagePostThumbnailRepository.FindListAsync(Builders<MessagePostThumbnail>.Filter.Eq("conversation_id", conversation.OId))).OrderByDescending(x => x.CreatedDate).Take(1);
                var latestConversationActivity = (await messageConversationActivityRepository.FindListAsync(Builders<MessageConversationActivity>.Filter.Eq("conversation_id", conversation.OId))).OrderByDescending(x => x.CreatedDate).Take(1);
                var latestMultiMediaMessage = (await messageMultiMediaRepository.FindListAsync(Builders<MessageMultiMedia>.Filter.Eq("conversation_id", conversation.OId))).OrderByDescending(x => x.CreatedDate).Take(1);

                var messageVM = new List<MessageViewModel>();
                messageVM.AddRange(mapper.Map<List<MessageViewModel>>(latestTextMessage));
                messageVM.AddRange(mapper.Map<List<MessageViewModel>>(latestImageMessage));
                messageVM.AddRange(mapper.Map<List<MessageViewModel>>(latestPostThumbnailMessage));
                messageVM.AddRange(mapper.Map<List<MessageViewModel>>(latestPostThumbnailMessage));
                messageVM.AddRange(mapper.Map<List<MessageViewModel>>(latestMultiMediaMessage));


                var conversationViewModel = mapper.Map<ConversationViewModel>(conversation);

                listData.Add(new ConversationData()
                {
                    Conversation = conversationViewModel,
                    Messages = messageVM.Take(1)
                });
            }
            return new GetConversationByUserIdResponse()
            {
                Conversations = listData
            };
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
        /// Adds the member.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Bạn không có quyền thêm người dùng mới vào cuộc trò chuyện.</exception>
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

using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
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
    public class MessageService : IMessageService
    {
        IMessageRepository messageRepository;
        IUserRepository userRepository;
        IHttpContextAccessor httpContextAccessor;
        IFcmRepository fcmRepository;
        IMapper mapper;
        IMessageConversationActivityRepository messageConversationActivityRepository;
        IMessageImageRepository messageImageRepository;
        IMessageMultiMediaRepository messageMultiMediaRepository;
        IMessagePostThumbnailRepository messagePostThumbnailRepository;
        IMessageTextRepository messageTextRepository;

        public MessageService(IMessageRepository messageRepository,
          IUserRepository userRepository,
          IHttpContextAccessor httpContextAccessor,
          IFcmRepository fcmRepository, IMapper mapper,
          IMessageConversationActivityRepository messageConversationActivityRepository,
          IMessageImageRepository messageImageRepository,
          IMessageMultiMediaRepository messageMultiMediaRepository,
          IMessagePostThumbnailRepository messagePostThumbnailRepository,
          IMessageTextRepository messageTextRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.fcmRepository = fcmRepository;
            this.mapper = mapper;
            this.messageConversationActivityRepository = messageConversationActivityRepository;
            this.messageImageRepository = messageImageRepository;
            this.messageMultiMediaRepository = messageMultiMediaRepository;
            this.messagePostThumbnailRepository = messagePostThumbnailRepository;
            this.messageTextRepository = messageTextRepository;
        }

        public async Task<MessageViewModel> AddMessage(AddMessageRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            switch (request.MessageType)
            {
                case MessageBaseType.Text:
                    {
                        MessageText message = new MessageText()
                        {
                            ConversationId = request.ConversationId,
                            Content = request.Content,
                            SenderId = currentUser.OId,
                            MessageType = request.MessageType
                        };

                        await messageTextRepository.AddAsync(message);
                        var result = mapper.Map<MessageViewModel>(message);
                        await fcmRepository.SendMessage(request.ConversationId, result);

                        return result;
                    }
                case MessageBaseType.PostThumbnail:
                    {
                        MessagePostThumbnail message = new MessagePostThumbnail()
                        {
                            ConversationId = request.ConversationId,
                            PostId = request.PostId,
                            MessageType = request.MessageType,
                            SenderId = currentUser.OId,
                        };

                        await messagePostThumbnailRepository.AddAsync(message);
                        var result = mapper.Map<MessageViewModel>(message);
                        await fcmRepository.SendMessage(request.ConversationId, result);
                        return result;
                    }
                case MessageBaseType.Image:
                    {
                        MessageImage message = new MessageImage()
                        {
                            ConversationId = request.ConversationId,
                            MessageType = request.MessageType,
                            Image = request.Image,
                            SenderId = currentUser.OId
                        };

                        await messageImageRepository.AddAsync(message);
                        var result = mapper.Map<MessageViewModel>(message);
                        await fcmRepository.SendMessage(request.ConversationId, result);
                        return result;
                    }
                case MessageBaseType.MultiMedia:
                    {
                        MessageMultiMedia message = new MessageMultiMedia()
                        {
                            ConversationId = request.ConversationId,
                            MessageType = request.MessageType,
                            MediaUrl = request.MediaUrl,
                            SenderId = currentUser.OId
                        };

                        await messageMultiMediaRepository.AddAsync(message);
                        var result = mapper.Map<MessageViewModel>(message);
                        await fcmRepository.SendMessage(request.ConversationId, result);
                        return result;
                    }

                case MessageBaseType.ConversationActivity:
                    {
                        MessageConversationActivity message = new MessageConversationActivity()
                        {
                            ConversationId = request.ConversationId,
                            ActivityDetail = request.ActivityDetail,
                            MessageType = request.MessageType,
                            SenderId = currentUser.OId,
                        };

                        await messageConversationActivityRepository.AddAsync(message);
                        var result = mapper.Map<MessageViewModel>(message);
                        await fcmRepository.SendMessage(request.ConversationId, result);
                        return result;
                    }
                default:
                    break;
            }

            return null;
        }

        public async Task<IEnumerable<MessageViewModel>> GetMessageByConversationId(GetMessageByConversationIdRequest request)
        {
            try
            {
                var result = new List<MessageViewModel>();

                var messageTextBuilder = Builders<MessageText>.Filter;
                var messageTextFilter = messageTextBuilder.Eq("conversation_id", request.ConversationId) & messageTextBuilder.Eq("status", ItemStatus.Active);
                var messageTexts = await messageTextRepository.FindListAsync(messageTextFilter);
                var messageTextViewModels = mapper.Map<List<MessageViewModel>>(messageTexts.OrderByDescending(x => x.CreatedDate));
                result.AddRange(messageTextViewModels);

                var messageImageBuilder = Builders<MessageImage>.Filter;
                var messageImageFilter = messageImageBuilder.Eq("conversation_id", request.ConversationId) & messageImageBuilder.Eq("status", ItemStatus.Active);
                var messageImages = await messageImageRepository.FindListAsync(messageImageFilter);
                var messageImageViewModels = mapper.Map<List<MessageViewModel>>(messageImages.OrderByDescending(x => x.CreatedDate));
                result.AddRange(messageImageViewModels);

                var messagePostThumbnailBuilder = Builders<MessagePostThumbnail>.Filter;
                var messagePostThumbnailFilter = messagePostThumbnailBuilder.Eq("conversation_id", request.ConversationId) & messagePostThumbnailBuilder.Eq("status", ItemStatus.Active);
                var messagePostThumnbails = await messagePostThumbnailRepository.FindListAsync(messagePostThumbnailFilter);
                var messagePostThumbnailViewModels = mapper.Map<List<MessageViewModel>>(messagePostThumnbails.OrderByDescending(x => x.CreatedDate));
                result.AddRange(messagePostThumbnailViewModels);

                var messageMultiMediaBuilder = Builders<MessageMultiMedia>.Filter;
                var messageMultiMediaFilter = messageMultiMediaBuilder.Eq("conversation_id", request.ConversationId) & messageMultiMediaBuilder.Eq("status", ItemStatus.Active);
                var messageMultiMedias = await messageMultiMediaRepository.FindListAsync(messageMultiMediaFilter);
                var messageMultiMediaViewModels = mapper.Map<List<MessageViewModel>>(messageMultiMedias.OrderByDescending(x => x.CreatedDate));
                result.AddRange(messageMultiMediaViewModels);

                result = result.OrderByDescending(x => x.CreatedDate).ToList();

                if (request.Skip.HasValue && request.Count.HasValue)
                {
                    result = result.Skip(request.Skip.Value).Take(request.Count.Value).ToList();
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Message> GetAll()
        {
            return messageRepository.GetAll().ToList();
        }


        public async Task<string> DeleteMessage(string id)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var existMessageConversationActivity = await messageConversationActivityRepository.GetByIdAsync(ObjectId.Parse(id));
            if (existMessageConversationActivity != null && existMessageConversationActivity.SenderId == currentUser.OId)
            {
                existMessageConversationActivity.Status = ItemStatus.Deleted;
                await messageConversationActivityRepository.UpdateAsync(existMessageConversationActivity, existMessageConversationActivity.Id);
                return "Xóa tin nhắn thành công";
            }

            var existMessageImage = await messageImageRepository.GetByIdAsync(ObjectId.Parse(id));
            if (existMessageImage != null && existMessageImage.SenderId == currentUser.OId)
            {
                existMessageImage.Status = ItemStatus.Deleted;
                await messageImageRepository.UpdateAsync(existMessageImage, existMessageImage.Id);
                return "Xóa tin nhắn thành công";
            }

            var existMessageMultiMedia = await messageMultiMediaRepository.GetByIdAsync(ObjectId.Parse(id));
            if (existMessageMultiMedia != null && existMessageMultiMedia.SenderId == currentUser.OId)
            {
                existMessageMultiMedia.Status = ItemStatus.Deleted;
                await messageMultiMediaRepository.UpdateAsync(existMessageMultiMedia, existMessageMultiMedia.Id);
                return ("Xóa tin nhắn thành công");
            }

            var existMessagePostThumbnail = await messagePostThumbnailRepository.GetByIdAsync(ObjectId.Parse(id));
            if (existMessagePostThumbnail != null && existMessagePostThumbnail.SenderId == currentUser.OId)
            {
                existMessagePostThumbnail.Status = ItemStatus.Deleted;
                await messagePostThumbnailRepository.UpdateAsync(existMessagePostThumbnail, existMessagePostThumbnail.Id);
                return ("Xóa tin nhắn thành công");
            }
            var existMessageText = await messageTextRepository.GetByIdAsync(ObjectId.Parse(id));
            if (existMessageText != null && existMessageText.SenderId == currentUser.OId)
            {
                existMessageText.Status = ItemStatus.Deleted;
                await messageTextRepository.UpdateAsync(existMessageText, existMessageText.Id);
                return ("Xóa tin nhắn thành công");
            }

            else
            {
                throw new Exception("Đã có lỗi xảy ra");
            }
        }

        public async Task<MessageViewModel> EditMessage(UpdateMessageRequest request)
        {
            Message message = await messageRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (message == null)
            {
                throw new Exception("Tin nhắn không tìm thấy");
            }

            message.MediaContent = request.Image;
            message.StringContent = request.Content;
            message.IsEdited = true;
            await messageRepository.UpdateAsync(message, message.Id);
            return mapper.Map<MessageViewModel>(message);
        }
    }
}


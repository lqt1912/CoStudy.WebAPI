using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant.NotificationConstant;
using static Common.Constants;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public class NofticationService : INofticationService
    {
        INofticationRepository nofticationRepository;

        IUserRepository userRepository;

        IHttpContextAccessor contextAccessor;

        IMapper mapper;

        INotificationTypeRepository notificationTypeRepository;

        INotificationDetailRepository notificationDetailRepository;

        INotificationObjectRepository notificationObjectRepository;

        public NofticationService(INofticationRepository nofticationRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor, IMapper mapper, INotificationTypeRepository notificationTypeRepository, INotificationDetailRepository notificationDetailRepository, INotificationObjectRepository notificationObjectRepository)
        {
            this.nofticationRepository = nofticationRepository;
            this.userRepository = userRepository;
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
            this.notificationTypeRepository = notificationTypeRepository;
            this.notificationDetailRepository = notificationDetailRepository;
            this.notificationObjectRepository = notificationObjectRepository;
        }

        public async Task<NotificationViewModel> AddNoftication(AddNofticationRequest request)
        {

            Noftication noftication = NofticationAdapter.FromRequest(request);
            await nofticationRepository.AddAsync(noftication);

            return mapper.Map<NotificationViewModel>(noftication);
        }

        public async Task<IEnumerable<NotificationViewModel>> GetCurrentUserNoftication(BaseGetAllRequest request)
        {
            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            FilterDefinition<Noftication> builder = Builders<Noftication>.Filter.Eq(ReceiverId, currentUser.OId);

            IEnumerable<Noftication> result = (await nofticationRepository.FindListAsync(builder)).AsEnumerable();

            //var tempObjectIdList = result.Select(x => x.ObjectId).Distinct();


            if (request.Count.HasValue && request.Skip.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return mapper.Map<IEnumerable<NotificationViewModel>>(result);
        }

        public async Task<string> DeleteNotification(string id)
        {
            try
            {
                await nofticationRepository.DeleteAsync(ObjectId.Parse(id));
                return DeleteNotificationSuccess;
            }
            catch (Exception)
            {
                return DeleteNotificationNotSuccess;
            }
        }

        public async Task<string> MarkAsRead(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Noftication notification = await nofticationRepository.GetByIdAsync(ObjectId.Parse(id));
                if (notification == null)
                {
                    throw new Exception(NotificationNotFound);
                }

                notification.IsRead = true;
                await nofticationRepository.UpdateAsync(notification, notification.Id);

                return NotificationSeen;
            }
            else
            {
                User currentUser = Feature.CurrentUser(contextAccessor, userRepository);
                FilterDefinitionBuilder<Noftication> builder = Builders<Noftication>.Filter;
                FilterDefinition<Noftication> finder = builder.Eq(OwnerId, currentUser.OId) & builder.Eq(IsRead, false) & builder.Eq(Status, ItemStatus.Active);
                List<Noftication> notis = await nofticationRepository.FindListAsync(finder);
                foreach (Noftication noti in notis)
                {
                    noti.IsRead = true;
                    await nofticationRepository.UpdateAsync(noti, noti.Id);
                }
                return NotificationSeenAll;
            }
        }

        public async Task<NotificationType> AddNotificationType(NotificationType entity)
        {
            NotificationType request = new NotificationType()
            {
                Code = entity.Code,
                ObjectType = entity.ObjectType,
                ContentTemplate = entity.ContentTemplate
            };

            await notificationTypeRepository.AddAsync(request);
            return request;
        }

        public async Task<NotificationType> GetByCode(string code)
        {
            FilterDefinition<NotificationType> finder = Builders<NotificationType>.Filter.Eq(Code, code);
            return await notificationTypeRepository.FindAsync(finder);
        }

        public async Task<IEnumerable<NotificationViewModel>> GetCurrentUserNotificationList()
        {
            List<NotificationViewModel> result = new List<NotificationViewModel>();

            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);

            IQueryable<string> notificationObjectIds = notificationObjectRepository.GetAll().Select(x => x.OId);

            foreach (string notificationObjectId in notificationObjectIds)
            {
                NotificationObject notificationObject = await notificationObjectRepository.GetByIdAsync(ObjectId.Parse(notificationObjectId));

                List<NotificationDetail> notificationDetails = notificationDetailRepository.GetAll().Where(
                    x => x.NotificationObjectId == notificationObjectId
                    && x.ReceiverId == currentUser.OId
                    && x.IsDeleted == false).OrderByDescending(x => x.ModifiedDate).ToList();


                NotificationDetail isNotRead = notificationDetails.FirstOrDefault(x => x.IsRead == false);

                if (notificationDetails.Any())
                {
                    User creator = (await userRepository.GetByIdAsync(ObjectId.Parse(notificationDetails.ElementAt(0).CreatorId)));

                    string ownerName = String.Empty;

                    FilterDefinition<NotificationType> notificationTypeFilter = Builders<NotificationType>.Filter.Eq(Code, notificationObject.NotificationType);

                    NotificationType notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

                    var notificationViewModelType = PushedNotificationType.Other;

                    switch (notificationType.Code)
                    {
                        case "ADD_POST_NOTIFY":
                        case "UPVOTE_POST_NOTIFY":
                        case "DOWNVOTE_POST_NOTIFY":
                            notificationViewModelType = PushedNotificationType.Post;
                            break;

                        case "UPVOTE_COMMENT_NOTIFY":
                        case "DOWNVOTE_COMMENT_NOTIFY":
                        case "COMMENT_NOTIFY":
                            notificationViewModelType = PushedNotificationType.Comment;
                            break;

                        case "UPVOTE_REPLY_NOTIFY":
                        case "DOWNVOTE_REPLY_NOTIFY":
                        case "REPLY_COMMENT_NOTIFY":
                            notificationViewModelType = PushedNotificationType.Reply;
                            break;

                        case "FOLLOW_NOTIFY":
                            notificationViewModelType = PushedNotificationType.User;
                            break;

                        default:
                            notificationViewModelType = PushedNotificationType.Other;
                            break;
                    }


                    if (notificationType.Code != AddPostNotify)
                    {
                        if (currentUser.OId != notificationObject.OwnerId)
                        {
                            User own = await userRepository.GetByIdAsync(ObjectId.Parse(notificationObject.OwnerId));
                            ownerName = $"{own?.FirstName} {own?.LastName}";
                        }
                        else
                        {
                            ownerName = "bạn";
                        }
                    }

                    if (notificationDetails.Count() == 1)
                    {
                        NotificationViewModel notificationViewModel = new NotificationViewModel()
                        {
                            AuthorAvatar = creator.AvatarHash,
                            AuthorId = creator.OId,
                            AuthorName = $"{creator.FirstName} {creator.LastName}",
                            Content = $"{creator.FirstName} { creator.LastName } {notificationType.ContentTemplate } {ownerName}",
                            IsRead = (isNotRead == null),
                            OwnerId = notificationObject.OwnerId,
                            ObjectId = notificationObject.ObjectId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Status = ItemStatus.Active,
                            OId = notificationObject.OId,
                            NotificationType = notificationViewModelType
                        };

                        result.Add(notificationViewModel);
                    }
                    else
                    {
                        NotificationViewModel notificationViewModel = new NotificationViewModel()
                        {
                            AuthorAvatar = creator.AvatarHash,
                            AuthorId = creator.OId,
                            AuthorName = $"{creator.FirstName} {creator.LastName}",
                            Content = $" {creator.FirstName} {creator.LastName} và {notificationDetails.Count() - 1} người khác {notificationType.ContentTemplate } {ownerName}",
                            IsRead = (isNotRead == null),
                            OwnerId = notificationObject.OwnerId,
                            ObjectId = notificationObject.ObjectId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Status = ItemStatus.Active,
                            OId = notificationObject.OId,
                            NotificationType = notificationViewModelType
                        };

                        result.Add(notificationViewModel);
                    }
                }
            }

            return result.OrderByDescending(x => x.ModifiedDate);
        }

        public async Task<string> DeleteCurrentNotification(string notificationObjectId)
        {
            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);

            FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;

            FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq(IsDeleted, false)
                & notificationDetailBuilder.Eq(NotificationObjectId, notificationObjectId)
                & notificationDetailBuilder.Eq(ReceiverId, currentUser.OId);

            List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);

            foreach (NotificationDetail notificationDetail in notificationDetails)
            {
                notificationDetail.IsDeleted = true;
                await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
            }

            return DeleteNotificationSuccess;
        }

        public async Task<string> MarkNotificaionsAsRead(string notificationObjectId)
        {

            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);

            if (!string.IsNullOrEmpty(notificationObjectId))
            {
                FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;

                FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq(IsRead, false)
                    & notificationDetailBuilder.Eq(IsDeleted, false)
                    & notificationDetailBuilder.Eq(NotificationObjectId, notificationObjectId)
                    & notificationDetailBuilder.Eq(ReceiverId, currentUser.OId);

                List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);

                foreach (NotificationDetail notificationDetail in notificationDetails)
                {
                    notificationDetail.IsRead = true;
                    await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
                }
                return NotificationSeen;
            }
            else
            {
                FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;

                FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq(IsRead, false)
                    & notificationDetailBuilder.Eq(IsDeleted, false)
                    & notificationDetailBuilder.Eq(ReceiverId, currentUser.OId);

                List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);

                foreach (NotificationDetail notificationDetail in notificationDetails)
                {
                    notificationDetail.IsRead = true;
                    await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
                }

                return NotificationSeenAll;
            }

        }
    }
}

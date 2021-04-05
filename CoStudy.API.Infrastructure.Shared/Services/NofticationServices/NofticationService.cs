using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.NofticationServices.INofticationService" />
    public class NofticationService : INofticationService
    {
        /// <summary>
        /// The noftication repository
        /// </summary>
        INofticationRepository nofticationRepository;

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The context accessor
        /// </summary>
        IHttpContextAccessor contextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The notification type repository
        /// </summary>
        INotificationTypeRepository notificationTypeRepository;

        /// <summary>
        /// The notification detail repository
        /// </summary>
        INotificationDetailRepository notificationDetailRepository;

        /// <summary>
        /// The notification object repository
        /// </summary>
        INotificationObjectRepository notificationObjectRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NofticationService"/> class.
        /// </summary>
        /// <param name="nofticationRepository">The noftication repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="notificationTypeRepository">The notification type repository.</param>
        /// <param name="notificationDetailRepository">The notification detail repository.</param>
        /// <param name="notificationObjectRepository">The notification object repository.</param>
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

        /// <summary>
        /// Adds the noftication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<NotificationViewModel> AddNoftication(AddNofticationRequest request)
        {

            Noftication noftication = NofticationAdapter.FromRequest(request);
            await nofticationRepository.AddAsync(noftication);

            return mapper.Map<NotificationViewModel>(noftication);
        }

        /// <summary>
        /// Gets the current user noftication.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public async Task<IEnumerable<NotificationViewModel>> GetCurrentUserNoftication(BaseGetAllRequest request)
        {
            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            FilterDefinition<Noftication> builder = Builders<Noftication>.Filter.Eq("receiver_id", currentUser.OId);

            IEnumerable<Noftication> result = (await nofticationRepository.FindListAsync(builder)).AsEnumerable();

            //var tempObjectIdList = result.Select(x => x.ObjectId).Distinct();


            if (request.Count.HasValue && request.Skip.HasValue)
            {
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return mapper.Map<IEnumerable<NotificationViewModel>>(result);
        }

        /// <summary>
        /// Deletes the notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<string> DeleteNotification(string id)
        {
            try
            {
                await nofticationRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch (Exception)
            {
                return "Xóa không thành công";
            }
        }

        /// <summary>
        /// Marks as read.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy thông báo</exception>
        public async Task<string> MarkAsRead(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Noftication notification = await nofticationRepository.GetByIdAsync(ObjectId.Parse(id));
                if (notification == null)
                {
                    throw new Exception("Không tìm thấy thông báo");
                }

                notification.IsRead = true;
                await nofticationRepository.UpdateAsync(notification, notification.Id);

                return "Đã xem thông báo";
            }
            else
            {
                User currentUser = Feature.CurrentUser(contextAccessor, userRepository);
                FilterDefinitionBuilder<Noftication> builder = Builders<Noftication>.Filter;
                FilterDefinition<Noftication> finder = builder.Eq("owner_id", currentUser.OId) & builder.Eq("is_read", false) & builder.Eq("status", ItemStatus.Active);
                List<Noftication> notis = await nofticationRepository.FindListAsync(finder);
                foreach (Noftication noti in notis)
                {
                    noti.IsRead = true;
                    await nofticationRepository.UpdateAsync(noti, noti.Id);
                }
                return "Đã xem tất cả thông báo";
            }
        }

        /// <summary>
        /// Adds the type of the notification.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<NotificationType> AddNotificationType(NotificationType entity)
        {
            NotificationType request = new NotificationType()
            {
                Code = entity.Code,
                ContentTemplate = entity.ContentTemplate
            };

            await notificationTypeRepository.AddAsync(request);
            return request;
        }

        /// <summary>
        /// Gets the by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public async Task<NotificationType> GetByCode(string code)
        {
            FilterDefinition<NotificationType> finder = Builders<NotificationType>.Filter.Eq("code", code);
            return await notificationTypeRepository.FindAsync(finder);
        }

        /// <summary>
        /// Gets the current user notification list.
        /// </summary>
        /// <returns></returns>
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

                if (notificationDetails.Count() > 0)
                {
                    User creator = (await userRepository.GetByIdAsync(ObjectId.Parse(notificationDetails.ElementAt(0).CreatorId)));

                    string ownerName = String.Empty;
                    if (currentUser.OId != notificationObject.OwnerId)
                    {
                        User own = await userRepository.GetByIdAsync(ObjectId.Parse(notificationObject.OwnerId));
                        ownerName = $"{own?.FirstName} {own?.LastName}";
                    }
                    else
                    {
                        ownerName = "bạn";
                    }

                    FilterDefinition<NotificationType> notificationTypeFilter = Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                    NotificationType notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

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
                            OId = notificationObject.OId
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
                            OId = notificationObject.OId
                        };

                        result.Add(notificationViewModel);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes the current notification.
        /// </summary>
        /// <param name="notificationObjectId">The notification object identifier.</param>
        /// <returns></returns>
        public async Task<string> DeleteCurrentNotification(string notificationObjectId)
        {
            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);

            FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;
           
            FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq("is_deleted", false)
                & notificationDetailBuilder.Eq("notification_object_id", notificationObjectId)
                & notificationDetailBuilder.Eq("receiver_id", currentUser.OId);

            List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);
            
            foreach (NotificationDetail notificationDetail in notificationDetails)
            {
                notificationDetail.IsDeleted = true;
                await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
            }

            return "Xóa thành công. ";
        }

        /// <summary>
        /// Marks the notificaions as read.
        /// </summary>
        /// <param name="notificationObjectId">The notification object identifier.</param>
        /// <returns></returns>
        public async Task<string> MarkNotificaionsAsRead(string notificationObjectId)
        {

            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);

            if (!string.IsNullOrEmpty(notificationObjectId))
            {
                FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;

                FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq("is_read", false)
                    & notificationDetailBuilder.Eq("is_deleted", false)
                    & notificationDetailBuilder.Eq("notification_object_id", notificationObjectId)
                    & notificationDetailBuilder.Eq("receiver_id", currentUser.OId);

                List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);

                foreach (NotificationDetail notificationDetail in notificationDetails)
                {
                    notificationDetail.IsRead = true;
                    await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
                }
                return "Đã đánh dấu là xem. ";
            }
            else
            {
                FilterDefinitionBuilder<NotificationDetail> notificationDetailBuilder = Builders<NotificationDetail>.Filter;

                FilterDefinition<NotificationDetail> notificationDetailFilter = notificationDetailBuilder.Eq("is_read", false)
                    & notificationDetailBuilder.Eq("is_deleted", false)
                    & notificationDetailBuilder.Eq("receiver_id", currentUser.OId);

                List<NotificationDetail> notificationDetails = await notificationDetailRepository.FindListAsync(notificationDetailFilter);

                foreach (NotificationDetail notificationDetail in notificationDetails)
                {
                    notificationDetail.IsRead = true;
                    await notificationDetailRepository.UpdateAsync(notificationDetail, notificationDetail.Id);
                }

                return "Tất cả thông báo đã xem. ";
            }

        }
    }
}

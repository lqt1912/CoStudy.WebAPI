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

        public NofticationService(INofticationRepository nofticationRepository,
            IUserRepository userRepository,
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            INotificationTypeRepository notificationTypeRepository)
        {
            this.nofticationRepository = nofticationRepository;
            this.userRepository = userRepository;
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
            this.notificationTypeRepository = notificationTypeRepository;
        }

        public async Task<NotificationViewModel> AddNoftication(AddNofticationRequest request)
        {

            Noftication noftication = NofticationAdapter.FromRequest(request);
            await nofticationRepository.AddAsync(noftication);

            return mapper.Map<NotificationViewModel>(noftication);
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

        public async Task<IEnumerable<NotificationViewModel>> GetCurrentUserNotificationList(GetAllNotificationRequest request)
        {
            var currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            var builder = Builders<Noftication>.Filter;
            var filter = builder.Eq("receiver_id", currentUser.OId)
                & (builder.Ne("author_id", currentUser.OId) | builder.Regex("content", "quản trị viên") | builder.Regex("content", "vi phạm quy định"))
                & builder.Eq("status", ItemStatus.Active);

            var notifications = (await nofticationRepository.FindListAsync(filter)).AsEnumerable();

            if (request.IsRead == false)
                notifications = notifications.Where(x => x.IsRead == false);

            if (request.Skip.HasValue && request.Count.HasValue)
                notifications = notifications.Skip(request.Skip.Value).Take(request.Count.Value);

            var result = mapper.Map<List<NotificationViewModel>>(notifications);

            return result.OrderByDescending(x => x.ModifiedDate);
        }

        public async Task<string> DeleteCurrentNotification(string notificationObjectId)
        {
            try
            {
                var exist = await nofticationRepository.GetByIdAsync(ObjectId.Parse(notificationObjectId));
                if (exist != null)
                    exist.Status = ItemStatus.Deleted;
                await nofticationRepository.UpdateAsync(exist, exist.Id);
                return DeleteNotificationSuccess;
            }
            catch (Exception)
            {
                return DeleteNotificationNotSuccess;
            }
        }

        public async Task<string> MarkNotificaionsAsRead(string notificationObjectId)
        {

            User currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            if (!string.IsNullOrEmpty(notificationObjectId))
            {
                var exist = await nofticationRepository.GetByIdAsync(ObjectId.Parse(notificationObjectId));
                if (exist != null)
                {
                    exist.IsRead = true;
                    await nofticationRepository.UpdateAsync(exist, exist.Id);
                }
            }
            else
            {
                var builder = Builders<Noftication>.Filter;
                var filter = builder.Eq("receiver_id", currentUser.OId)
                    & builder.Eq("status", ItemStatus.Active);
                var notifications = await nofticationRepository.FindListAsync(filter);
                notifications.ForEach(async x =>
                {
                    x.IsRead = true;
                    await nofticationRepository.UpdateAsync(x, x.Id);
                });
            }
            return NotificationSeen;
        }

        public async Task<string> DeleteNotification()
        {
            var currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            var builder = Builders<Noftication>.Filter;
            var filter = builder.Eq("receiver_id", currentUser.OId);
            var notifications = await nofticationRepository.FindListAsync(filter);
            notifications.ForEach(async x =>
            {
                x.Status = ItemStatus.Deleted;
                await nofticationRepository.UpdateAsync(x, x.Id);
            });
            return "Xóa tất cả thông báo thành công. ";
        }
    }
}

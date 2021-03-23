using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
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
        /// Initializes a new instance of the <see cref="NofticationService"/> class.
        /// </summary>
        /// <param name="nofticationRepository">The noftication repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public NofticationService(INofticationRepository nofticationRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            this.nofticationRepository = nofticationRepository;
            this.userRepository = userRepository;
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
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
            FilterDefinition<Noftication> builder = Builders<Noftication>.Filter.Eq("owner_id", currentUser.OId);

            IEnumerable<Noftication> result = (await nofticationRepository.FindListAsync(builder)).AsEnumerable();

            if (request.Count.HasValue && request.Skip.HasValue)
                result = result.Skip(request.Skip.Value).Take(request.Count.Value);
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
                    throw new Exception("Không tìm thấy thông báo");
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
    }
}

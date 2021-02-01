using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public class NofticationService : INofticationService
    {
        INofticationRepository nofticationRepository;
        IUserRepository userRepository;
        IHttpContextAccessor contextAccessor;

        public NofticationService(INofticationRepository nofticationRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            this.nofticationRepository = nofticationRepository;
            this.userRepository = userRepository;
            this.contextAccessor = contextAccessor;
        }

        public async Task<AddNofticationResponse> AddNoftication(AddNofticationRequest request)
        {

            var noftication = NofticationAdapter.FromRequest(request);
            await nofticationRepository.AddAsync(noftication);
            return NofticationAdapter.ToResponse(noftication);
        }

        public async Task<IEnumerable<Noftication>> GetCurrentUserNoftication(int? skip, int? count)
        {
            var currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            var builder = Builders<Noftication>.Filter.Eq("owner_id", currentUser.OId);
            var result = (await nofticationRepository.FindListAsync(builder)).AsEnumerable();
            if (skip.HasValue && count.HasValue)
                result = result.Skip(skip.Value).Take(count.Value);
            return result;
        }

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

        public async Task<string> MarkAsRead(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var notification = await nofticationRepository.GetByIdAsync(ObjectId.Parse(id));
                if (notification == null)
                    throw new Exception("Không tìm thấy thông báo");
                notification.IsRead = true;
                await nofticationRepository.UpdateAsync(notification, notification.Id);

                return "Đã xem thông báo";
            }
            else
            {
                var currentUser = Feature.CurrentUser(contextAccessor, userRepository);
                var builder = Builders<Noftication>.Filter; 
                var finder = builder.Eq("owner_id", currentUser.OId)  & builder.Eq("is_read", false) & builder.Eq("status", ItemStatus.Active);
                var notis = await nofticationRepository.FindListAsync(finder);
                foreach (var noti in notis)
                {
                    noti.IsRead = true;
                    await nofticationRepository.UpdateAsync(noti, noti.Id);
                }
                return "Đã xem tất cả thông báo";
            }
        }
    }
}

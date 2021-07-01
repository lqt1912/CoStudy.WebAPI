using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public interface INofticationService
    {
        Task<NotificationViewModel> AddNoftication(AddNofticationRequest request);
        Task<NotificationType> AddNotificationType(NotificationType entity);
        Task<IEnumerable<NotificationViewModel>> GetCurrentUserNotificationList(BaseGetAllRequest request);
        Task<string> DeleteCurrentNotification(string notificationObjectId);
        Task<string> MarkNotificaionsAsRead(string notificationObjectId);
    }
}

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
             Task<IEnumerable<NotificationViewModel>> GetCurrentUserNoftication(BaseGetAllRequest request);

             Task<string> DeleteNotification(string id);



        Task<string> MarkAsRead(string id);


        #region New Class Notificaton        

             Task<NotificationType> AddNotificationType(NotificationType entity);


             Task<NotificationType> GetByCode(string code);

            Task<IEnumerable<NotificationViewModel>> GetCurrentUserNotificationList();

             Task<string> DeleteCurrentNotification(string notificationObjectId);

             Task<string> MarkNotificaionsAsRead(string notificationObjectId);

        #endregion
    }
}

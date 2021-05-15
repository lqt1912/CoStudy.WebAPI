using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    /// <summary>
    /// Interface INofiticationService
    /// </summary>
    public interface INofticationService
    {
        /// <summary>
        /// Adds the noftication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<NotificationViewModel> AddNoftication(AddNofticationRequest request);
        /// <summary>
        /// Gets the current user noftication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<NotificationViewModel>> GetCurrentUserNoftication(BaseGetAllRequest request);

        /// <summary>
        /// Deletes the notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string> DeleteNotification(string id);



        Task<string> MarkAsRead(string id);


        #region New Class Notificaton        

        /// <summary>
        /// Adds the type of the notification.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<NotificationType> AddNotificationType(NotificationType entity);


        /// <summary>
        /// Gets the by code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task<NotificationType> GetByCode(string code);

        /// <summary>
        /// Gets the current user notification list.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NotificationViewModel>> GetCurrentUserNotificationList();

        /// <summary>
        /// Deletes the current notification.
        /// </summary>
        /// <param name="notificationObjectId">The notification object identifier.</param>
        /// <returns></returns>
        Task<string> DeleteCurrentNotification(string notificationObjectId);

        /// <summary>
        /// Marks the notificaions as read.
        /// </summary>
        /// <param name="notificationObjectId">The notification object identifier.</param>
        /// <returns></returns>
        Task<string> MarkNotificaionsAsRead(string notificationObjectId);

        #endregion
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Threading.Tasks;

namespace CoStudy.API.Application.FCM
{
    public interface IFcmRepository
    {
        Task<FcmInfo> AddFcmInfo(string userId, string deviceToken);
        Task<FcmInfo> RevokeFcmInfo(string userId, string deviceToken);

        Task SendMessage(string clientGroupName, MessageViewModel message);
        Task PushNotify(string clientGroupName, Noftication noftication, Triple<string, string, ObjectNotificationType> notificationSetting);

        Task PushNotifyDetail(string clientGroupName, NotificationDetail notificationDetail);

        Task<string> SendNotification();

        Task AddToGroup(AddUserToGroupRequest request);
        Task RemoveFromGroup(RemoveFromGroupRequest request);

        Task PushNotifyReport(string userId, NotificationDetail notificationDetail);

        Task PushNotifyApproveReport(string userId, NotificationDetail notificationDetail);

        Task PushNotifyPostMatch(string userId, NotificationDetail notificationDetail);

    }
}

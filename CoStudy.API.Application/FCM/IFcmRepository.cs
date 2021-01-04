using CoStudy.API.Domain.Entities.Application;
using System.Threading.Tasks;

namespace CoStudy.API.Application.FCM
{
    public interface IFcmRepository
    {
        Task<FcmInfo> AddFcmInfo(string userId, string deviceToken);
        Task<FcmInfo> RevokeFcmInfo(string userId, string deviceToken);

        Task SendMessage(string clientGroupName, Message message);
        Task PushNotify(string clientGroupName, Noftication noftication);

        Task<string> SendNotification();
    }
}

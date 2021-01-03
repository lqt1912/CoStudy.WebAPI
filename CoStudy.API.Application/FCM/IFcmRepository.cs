using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Application.FCM
{
    public interface IFcmRepository
    {
        Task<FcmInfo> AddFcmInfo(string userId, string deviceToken);
        Task<FcmInfo> RevokeFcmInfo(string userId, string deviceToken);

        Task SendMessage(string clientGroupName, Message message);

        Task<string> SendNotification();
    }
}

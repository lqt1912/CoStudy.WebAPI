﻿using CoStudy.API.Domain.Entities.Application;
using System.Threading.Tasks;

namespace CoStudy.API.Application.FCM
{
    /// <summary>
    /// Interface FCM Repository
    /// </summary>
    public interface IFcmRepository
    {
        /// <summary>
        /// Adds the FCM information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        Task<FcmInfo> AddFcmInfo(string userId, string deviceToken);
        /// <summary>
        /// Revokes the FCM information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        Task<FcmInfo> RevokeFcmInfo(string userId, string deviceToken);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="clientGroupName">Name of the client group.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendMessage(string clientGroupName, Message message);
        /// <summary>
        /// Pushes the notify.
        /// </summary>
        /// <param name="clientGroupName">Name of the client group.</param>
        /// <param name="noftication">The noftication.</param>
        /// <returns></returns>
        Task PushNotify(string clientGroupName, Noftication noftication);

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <returns></returns>
        Task<string> SendNotification();
    }
}

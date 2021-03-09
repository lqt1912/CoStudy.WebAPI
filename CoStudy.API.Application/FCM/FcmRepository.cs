using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Application.FCM
{
    /// <summary>
    /// Fcm repository
    /// </summary>
    /// <seealso cref="CoStudy.API.Application.FCM.IFcmRepository" />
    public class FcmRepository : IFcmRepository
    {
        /// <summary>
        /// The FCM information repository
        /// </summary>
        IFcmInfoRepository fcmInfoRepository;
        /// <summary>
        /// The client group repository
        /// </summary>
        IClientGroupRepository clientGroupRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FcmRepository"/> class.
        /// </summary>
        /// <param name="fcmInfoRepository">The FCM information repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public FcmRepository(IFcmInfoRepository fcmInfoRepository, 
            IClientGroupRepository clientGroupRepository,
            IUserRepository userRepository, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.fcmInfoRepository = fcmInfoRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds the FCM information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public async Task<FcmInfo> AddFcmInfo(string userId, string deviceToken)
        {
            var finder = Builders<FcmInfo>.Filter.Eq("user_id", userId);
            var existFcmInfo = await fcmInfoRepository.FindAsync(finder);
            if (existFcmInfo != null)
            {
                existFcmInfo.DeviceToken = deviceToken;
                existFcmInfo.ModifiedDate = DateTime.Now;
                await fcmInfoRepository.UpdateAsync(existFcmInfo, existFcmInfo.Id);
                return existFcmInfo;
            }
            else
            {
                var newFcmInfo = new FcmInfo()
                {
                    UserId = userId,
                    DeviceToken = deviceToken,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                await fcmInfoRepository.AddAsync(newFcmInfo);
                return newFcmInfo;
            }
        }

        /// <summary>
        /// Revokes the FCM information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public async Task<FcmInfo> RevokeFcmInfo(string userId, string deviceToken)
        {
            var builder = Builders<FcmInfo>.Filter;
            var finder = builder.Eq("user_id", userId) & builder.Eq("device_token", deviceToken);
            var exist = await fcmInfoRepository.FindAsync(finder);
            if (exist != null)
            {
                exist.DeviceToken = string.Empty;
                exist.ModifiedDate = DateTime.Now;
                await fcmInfoRepository.UpdateAsync(exist, exist.Id);
                return exist;
            }
            else return null;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="clientGroupName">Name of the client group.</param>
        /// <param name="message">The message.</param>
        public async Task SendMessage(string clientGroupName, Domain.Entities.Application.Message message)
        {
            try
            {
                var finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                var clientGroup = await clientGroupRepository.FindAsync(finder);
                foreach (var item in clientGroup.UserIds)
                {
                    var user = Builders<FcmInfo>.Filter.Eq("user_id", item);
                    var token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                    var sender = await userRepository.GetByIdAsync(ObjectId.Parse(message.SenderId));
                    var mes = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = token,

                        Data = new Dictionary<string, string>()
                        {
                            { "message",  JsonConvert.SerializeObject(message) }

                        },

                        Notification = new Notification()
                        {
                            Title = sender.LastName,
                            Body = message.StringContent,
                            ImageUrl = sender.AvatarHash
                        }
                    };
                    var response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        /// <summary>
        /// Pushes the notify.
        /// </summary>
        /// <param name="clientGroupName">Name of the client group.</param>
        /// <param name="noftication">The noftication.</param>
        public async Task PushNotify(string clientGroupName, Noftication noftication)
        {
            try
            {
                var finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                var clientGroup = await clientGroupRepository.FindAsync(finder);
                foreach (var item in clientGroup.UserIds)
                {
                    var user = Builders<FcmInfo>.Filter.Eq("user_id", item);
                    var token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                    var sender = await userRepository.GetByIdAsync(ObjectId.Parse(noftication.AuthorId));
                    var mes = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = token,

                        Data = new Dictionary<string, string>()
                        {
                            { "notification",  JsonConvert.SerializeObject(noftication) }
                        },
                        Notification = new Notification()
                        {
                            Title = sender.LastName,
                            Body = noftication.Content,
                            ImageUrl = sender.AvatarHash
                        }
                    };
                    var response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendNotification()
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Token = "d2Yvrq1sT8iZyZvgKtaE2Z:APA91bGuJp5Cyt2J1WuPB-GhEA3sR1eG-CwnIFJn0E2BguDjenY4TDy9gyY0phVJ8VqCZL_3Z56gaG-tBoIXjPZGxmHRpcT_CsQbNi-OxJ-XCJDxQx9LFWGgvnVPxyNqPoTm50GzNmwz",

                Data = new Dictionary<string, string>()
                    {
                        {"title", "Title test"},
                        {"body", "Body Test"},
                    },
                Notification = new Notification()
                {
                    Title = "title test",
                    Body = "body test"
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message).ConfigureAwait(true);
            return response;
        }
    }
}

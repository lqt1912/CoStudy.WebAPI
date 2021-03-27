using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        IMapper mapper; 

        /// <summary>
        /// The noftication repository
        /// </summary>
        INofticationRepository nofticationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FcmRepository" /> class.
        /// </summary>
        /// <param name="fcmInfoRepository">The FCM information repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public FcmRepository(IFcmInfoRepository fcmInfoRepository,
            IClientGroupRepository clientGroupRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            INofticationRepository nofticationRepository, IMapper mapper)
        {
            this.fcmInfoRepository = fcmInfoRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.nofticationRepository = nofticationRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds the FCM information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns></returns>
        public async Task<FcmInfo> AddFcmInfo(string userId, string deviceToken)
        {
            FilterDefinition<FcmInfo> finder = Builders<FcmInfo>.Filter.Eq("user_id", userId);
            FcmInfo existFcmInfo = await fcmInfoRepository.FindAsync(finder);
            if (existFcmInfo != null)
            {
                existFcmInfo.DeviceToken = deviceToken;
                existFcmInfo.ModifiedDate = DateTime.Now;
                await fcmInfoRepository.UpdateAsync(existFcmInfo, existFcmInfo.Id);
                return existFcmInfo;
            }
            else
            {
                FcmInfo newFcmInfo = new FcmInfo()
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
            FilterDefinitionBuilder<FcmInfo> builder = Builders<FcmInfo>.Filter;
            FilterDefinition<FcmInfo> finder = builder.Eq("user_id", userId) & builder.Eq("device_token", deviceToken);
            FcmInfo exist = await fcmInfoRepository.FindAsync(finder);
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
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);
                foreach (string item in clientGroup.UserIds)
                {
                    FilterDefinition<FcmInfo> user = Builders<FcmInfo>.Filter.Eq("user_id", item);
                    string token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                    User sender = await userRepository.GetByIdAsync(ObjectId.Parse(message.SenderId));
                    FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
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
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
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
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);
                foreach (string receiver in clientGroup.UserIds)
                {
                    var finalNotification = new Noftication()
                    {
                        CreatedDate = DateTime.Now,
                        AuthorId = noftication.AuthorId,
                        ContentType = noftication.ContentType,
                        IsRead = false,
                        ModifiedDate = DateTime.Now,
                        OwnerId = noftication.OwnerId,
                        Status = ItemStatus.Active,
                        ReceiverId = receiver,
                        ObjectId = noftication.ObjectId
                    };

                    //Người tạo ra thông báo. 
                    var creator = noftication.AuthorId;
                    var userCreator = await userRepository.GetByIdAsync(ObjectId.Parse(creator));

                    //Chủ sở hữu post
                    var owner = noftication.OwnerId;
                    var userOwner = await userRepository.GetByIdAsync(ObjectId.Parse(owner));


                    if (owner == creator)
                    {
                        if (receiver == owner)
                        {
                            finalNotification.Content = Feature.BuildNotifyContent(noftication.ContentType, "Bạn", "chính mình");
                        }
                        else if (receiver != owner)
                        {
                            finalNotification.Content = Feature.BuildNotifyContent(noftication.ContentType, userCreator.LastName, "họ");
                        }
                    }
                    else if (owner != creator)
                    {
                        if (receiver != owner)
                        {
                            finalNotification.Content = Feature.BuildNotifyContent(noftication.ContentType, userCreator.LastName, userOwner.LastName);
                        }
                        else if (receiver != creator)
                        {
                            finalNotification.Content = Feature.BuildNotifyContent(noftication.ContentType, userCreator.LastName, "bạn");
                        }
                    }

                    await nofticationRepository.AddAsync(finalNotification);
                    if (!(owner == creator && receiver == owner))
                    {

                        FilterDefinition<FcmInfo> user = Builders<FcmInfo>.Filter.Eq("user_id", receiver);
                        string token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                        User sender = await userRepository.GetByIdAsync(ObjectId.Parse(noftication.AuthorId));
                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
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
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                    }
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
            FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message()
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
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message).ConfigureAwait(true);
            return response;
        }

        /// <summary>
        /// Adds to group.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task AddToGroup(AddUserToGroupRequest request)
        {
            if (request.UserIds == null)
                request.UserIds = new List<string>();

            var builder = Builders<ClientGroup>.Filter.Eq("name", request.GroupName);
            var clientGroup = await clientGroupRepository.FindAsync(builder);

            if (clientGroup != null)
            {
                foreach (var userId in request.UserIds)
                {
                    var _userId = clientGroup.UserIds.FirstOrDefault(x => x == userId);

                    if (string.IsNullOrEmpty(_userId))
                        clientGroup.UserIds.Add(userId);

                }
                await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
            }
            else
            {
                var group = new ClientGroup()
                {
                    Name = request.GroupName,
                    GroupType = request.Type
                };
                group.UserIds.AddRange(request.UserIds.Distinct());

                await clientGroupRepository.AddAsync(group);
            };
        }

        /// <summary>
        /// Removes from group.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task RemoveFromGroup(RemoveFromGroupRequest request)
        {
            if (request.UserIds == null)
                request.UserIds = new List<string>();

            var builder = Builders<ClientGroup>.Filter.Eq("name", request.GroupName);
            var clientGroup = await clientGroupRepository.FindAsync(builder);

            if (clientGroup != null)
            {
                foreach (var userId in request.UserIds)
                {
                    clientGroup.UserIds.Remove(userId);
                }
                await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
            }
            else
            {
                return;
            }
        }
    }
}

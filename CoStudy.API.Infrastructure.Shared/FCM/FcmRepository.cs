using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
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

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The noftication repository
        /// </summary>
        INofticationRepository nofticationRepository;

        /// <summary>
        /// The notification detail repository
        /// </summary>
        INotificationDetailRepository notificationDetailRepository;

        /// <summary>
        /// The notification object repository
        /// </summary>
        INotificationObjectRepository notificationObjectRepository;

        /// <summary>
        /// The notification type repository
        /// </summary>
        INotificationTypeRepository notificationTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FcmRepository" /> class.
        /// </summary>
        /// <param name="fcmInfoRepository">The FCM information repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="nofticationRepository">The noftication repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="notificationDetailRepository">The notification detail repository.</param>
        /// <param name="notificationObjectRepository">The notification object repository.</param>
        /// <param name="notificationTypeRepository">The notification type repository.</param>
        public FcmRepository(IFcmInfoRepository fcmInfoRepository,
            IClientGroupRepository clientGroupRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            INofticationRepository nofticationRepository,
            IMapper mapper, 
            INotificationDetailRepository notificationDetailRepository, 
            INotificationObjectRepository notificationObjectRepository, 
            INotificationTypeRepository notificationTypeRepository)
        {
            this.fcmInfoRepository = fcmInfoRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.nofticationRepository = nofticationRepository;
            this.mapper = mapper;
            this.notificationDetailRepository = notificationDetailRepository;
            this.notificationObjectRepository = notificationObjectRepository;
            this.notificationTypeRepository = notificationTypeRepository;
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
        public async Task SendMessage(string clientGroupName, MessageViewModel message)
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
                            Body = message.Content.ToString(),
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

        /// <summary>
        /// Pushes the notify detail.
        /// </summary>
        /// <param name="clientGroupName">Name of the client group.</param>
        /// <param name="notificationDetail">The notification detail.</param>
        public async Task PushNotifyDetail(string clientGroupName, NotificationDetail notificationDetail)
        {
            FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            foreach (string receiverId in clientGroup.UserIds)
            {
                var existNotificationDetailBuilder = Builders<NotificationDetail>.Filter;
                var existNotificationDetailFilter = existNotificationDetailBuilder.Eq("creator_id", notificationDetail.CreatorId)
                                                    & existNotificationDetailBuilder.Eq("receiver_id", receiverId)
                                                    & existNotificationDetailBuilder.Eq("notification_object_id", notificationDetail.NotificationObjectId);

                var existNotificationDetail = await notificationDetailRepository.FindAsync(existNotificationDetailFilter);

                var finalNotificationDetail = new NotificationDetail();
                if (existNotificationDetail != null)
                {
                    finalNotificationDetail = existNotificationDetail;
                    finalNotificationDetail.ModifiedDate = DateTime.Now;
                    finalNotificationDetail.IsDeleted = false;
                    await notificationDetailRepository.UpdateAsync(finalNotificationDetail, finalNotificationDetail.Id);
                }
                else  {
                    finalNotificationDetail.CreatorId = notificationDetail.CreatorId;
                    finalNotificationDetail.ReceiverId = receiverId;
                    finalNotificationDetail.NotificationObjectId = notificationDetail.NotificationObjectId;
                    await notificationDetailRepository.AddAsync(finalNotificationDetail);
                };

                var notificationObject = await notificationObjectRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.NotificationObjectId));

                var owner = await userRepository.GetByIdAsync(ObjectId.Parse(notificationObject.OwnerId));

                var receiver = await userRepository.GetByIdAsync(ObjectId.Parse(receiverId));

                var creator = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.CreatorId));

                var notificationTypeFilter = Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                var notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

                var notifyContent = string.Empty;

                if (owner == creator)
                {
                    if (receiver == owner)
                    {
                        notifyContent = $"Bạn {notificationType.ContentTemplate} chính mình";
                    }
                    else if (receiver != owner)
                    {
                        notifyContent = $"{creator.LastName} {notificationType.ContentTemplate} họ";
                    }
                }
                else if (owner != creator)
                {
                    if (receiver != owner)
                    {
                        notifyContent = $"{creator.LastName} {notificationType.ContentTemplate} {owner.LastName}";
                    }
                    else if (receiver != creator)
                    {
                        notifyContent = $"{creator.LastName} {notificationType.ContentTemplate} bạn";
                    }
                }

                var notificationToPush = new PushedNotificationViewModel()
                {
                    AuthorName = $"{creator.FirstName} {creator.LastName}",
                    AuthorAvatar = creator.AvatarHash,
                    AuthorId = creator.OId,
                    Content = notifyContent,
                    CreatedDate = DateTime.Now,
                    IsRead = false,
                    ModifiedDate = DateTime.Now,
                    ObjectId = notificationObject.ObjectId,
                    OId = notificationObject.OId,
                    OwnerId = owner.OId,
                    Status = ItemStatus.Active
                };


                try
                {
                    if (!(owner == creator && receiver == owner))
                    {
                        FilterDefinition<FcmInfo> userFilter = Builders<FcmInfo>.Filter.Eq("user_id", receiverId);
                        string token = (await fcmInfoRepository.FindAsync(userFilter)).DeviceToken;

                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                        {
                            Token = token,
                            Data = new Dictionary<string, string>()
                        {
                            { "notification",  JsonConvert.SerializeObject(notificationToPush) }
                        },
                            Notification = new Notification()
                            {
                                Title = creator.LastName,
                                Body = notifyContent,
                                ImageUrl = creator.AvatarHash
                            }
                        };
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                    }
                }
                catch (Exception)
                {
                    // do nothing 
                }
            }
        }
    }
}

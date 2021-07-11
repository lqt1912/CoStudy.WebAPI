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
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace CoStudy.API.Application.FCM
{
    public class FcmRepository : IFcmRepository
    {
        IFcmInfoRepository fcmInfoRepository;

        IClientGroupRepository clientGroupRepository;

        IUserRepository userRepository;

        IHttpContextAccessor httpContextAccessor;

        IMapper mapper;

        INofticationRepository nofticationRepository;

        INotificationDetailRepository notificationDetailRepository;

        INotificationObjectRepository notificationObjectRepository;

        INotificationTypeRepository notificationTypeRepository;
        IConfiguration configuration;
        public FcmRepository(IFcmInfoRepository fcmInfoRepository,
            IClientGroupRepository clientGroupRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            INofticationRepository nofticationRepository,
            IMapper mapper,
            INotificationDetailRepository notificationDetailRepository,
            INotificationObjectRepository notificationObjectRepository,
            INotificationTypeRepository notificationTypeRepository, IConfiguration configuration)
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
            this.configuration = configuration;
        }

        public async Task<FcmInfo> AddFcmInfo(string userId, string deviceToken)
        {
            FilterDefinition<FcmInfo> finder = Builders<FcmInfo>.Filter.Eq("user_id", userId);
            FcmInfo existFcmInfo = await fcmInfoRepository.FindAsync(finder);
            if (existFcmInfo != null)
            {
                //Remove other 
                FilterDefinitionBuilder<FcmInfo> _builder = Builders<FcmInfo>.Filter;
                FilterDefinition<FcmInfo> _finder = _builder.Eq("device_token", deviceToken);
                var exist = await fcmInfoRepository.FindListAsync(_finder);
                exist.ForEach(async x =>
                {
                    x.DeviceToken = string.Empty;
                    await fcmInfoRepository.UpdateAsync(x, x.Id);
                });

                existFcmInfo.DeviceToken = deviceToken;
                existFcmInfo.ModifiedDate = DateTime.Now;
                await fcmInfoRepository.UpdateAsync(existFcmInfo, existFcmInfo.Id);
                existFcmInfo.IsActive = true;
                return existFcmInfo;
            }
            else
            {
                FcmInfo newFcmInfo = new FcmInfo()
                {
                    UserId = userId,
                    DeviceToken = deviceToken,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                await fcmInfoRepository.AddAsync(newFcmInfo);
                return newFcmInfo;
            }
        }

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
            else
            {
                return null;
            }
        }

        public async Task SendMessage(string clientGroupName, MessageViewModel message)
        {
            FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);
            foreach (string item in clientGroup.UserIds)
            {
                try
                {
                    FilterDefinition<FcmInfo> user = Builders<FcmInfo>.Filter.Eq("user_id", item);
                    string token = (await fcmInfoRepository.FindAsync(user))?.DeviceToken;
                    User sender = await userRepository.GetByIdAsync(ObjectId.Parse(message.SenderId));

                    var notifyBody = string.Empty;
                    switch (message.MessageType.Value)
                    {
                        case MessageBaseType.Text:
                            notifyBody = (message.Content as List<string>)[0];
                            break;
                        case MessageBaseType.Image:
                            notifyBody = "Đã gửi một ảnh. ";
                            break;
                        case MessageBaseType.MultiMedia:
                            notifyBody = "Đã gửi một phương tiện. ";
                            break;
                        case MessageBaseType.PostThumbnail:
                            notifyBody = "Đã gửi một liên kết. ";
                            break;
                        case MessageBaseType.ConversationActivity:
                            notifyBody = "Đã có hoạt động mới. ";
                            break;
                        case MessageBaseType.Other:
                            notifyBody = "Đã có thông báo mới. ";
                            break;
                        default:
                            break;
                    }

                    try
                    {
                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                        {
                            Token = token,
                            Data = new Dictionary<string, string>()
                        {
                            {"message", JsonConvert.SerializeObject(message)}
                        },
                            Notification = new Notification()
                            {
                                Title = sender.LastName,
                                Body = notifyBody,
                                ImageUrl = sender.AvatarHash
                            }
                        };
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                    }
                    catch (Exception)
                    {
                        //Do nothing
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public async Task PushNotify(string clientGroupName, Noftication noftication, Triple<string, string, ObjectNotificationType> notificationSetting)
        {
            try
            {
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);

                foreach (string receiver in clientGroup.UserIds)
                {
                    var receiverUser = await userRepository.GetByIdAsync(ObjectId.Parse(receiver));
                    if (receiverUser.TurnOfNotification != null)
                        if (receiverUser.TurnOfNotification.Any(x => x == noftication.ObjectId))
                            return;

                    Noftication finalNotification = new Noftication()
                    {
                        AuthorId = noftication.AuthorId,
                        OwnerId = noftication.OwnerId,
                        ReceiverId = receiver,
                        ObjectId = noftication.ObjectId,
                        ObjectType = notificationSetting.Item3,
                        ObjectThumbnail = noftication.ObjectThumbnail,
                    };

                    //Người tạo ra thông báo. 
                    string creator = noftication.AuthorId;
                    User userCreator = await userRepository.GetByIdAsync(ObjectId.Parse(creator));

                    //Chủ sở hữu post
                    string owner = noftication.OwnerId;
                    User userOwner = await userRepository.GetByIdAsync(ObjectId.Parse(owner));


                    if (owner == creator)
                    {
                        if (receiver == owner)
                        {
                            finalNotification.Content = $"Bạn {notificationSetting.Item2} chính mình";
                        }
                        else if (receiver != owner)
                        {
                            finalNotification.Content = $"{userCreator.FirstName} {userCreator.LastName} {notificationSetting.Item2} họ. ";
                        }
                    }
                    else if (owner != creator)
                    {
                        //owner: Trung 
                        //receiver: trung 
                        //Creator: Thắng
                        if (receiver != owner)
                        {
                            finalNotification.Content = $"{userCreator.FirstName} {userCreator.LastName} {notificationSetting.Item2} {userOwner.FirstName} {userOwner.LastName}. ";
                        }
                        else if (receiver != creator)
                        {
                            finalNotification.Content = $"{userCreator.FirstName} {userCreator.LastName} {notificationSetting.Item2} bạn. ";
                        }
                    }

                    await nofticationRepository.AddAsync(finalNotification);
                    var notiViewModel = mapper.Map<NotificationViewModel>(finalNotification);
                    if (!(owner == creator && receiver == owner && creator == receiver))
                    {
                        try
                        {
                            FilterDefinition<FcmInfo> user = Builders<FcmInfo>.Filter.Eq("user_id", receiver);
                            string token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                            User sender = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotification.AuthorId));
                            FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                            {
                                Token = token,

                                Data = new Dictionary<string, string>()
                            {
                                {"notification", JsonConvert.SerializeObject(notiViewModel)}
                            },
                                Notification = new Notification()
                                {
                                    Title = $"{sender.FirstName} {sender.LastName}",
                                    Body = notiViewModel.Content,
                                    ImageUrl = sender.AvatarHash
                                }
                            };
                            string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }
        public async Task PushNotify(string clientGroupName, Noftication noftication, Triple<string, string, ObjectNotificationType> notificationSetting, string customContent)
        {
            try
            {
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);
                foreach (string receiver in clientGroup.UserIds)
                {
                    Noftication finalNotification = new Noftication()
                    {
                        AuthorId = noftication.AuthorId,
                        OwnerId = noftication.OwnerId,
                        ReceiverId = receiver,
                        ObjectId = noftication.ObjectId,
                        ObjectType = notificationSetting.Item3,
                        ObjectThumbnail = noftication.ObjectThumbnail,
                        Content = customContent
                    };

                    await nofticationRepository.AddAsync(finalNotification);
                    var notiViewModel = mapper.Map<NotificationViewModel>(finalNotification);

                    try
                    {
                        FilterDefinition<FcmInfo> user = Builders<FcmInfo>.Filter.Eq("user_id", receiver);
                        string token = (await fcmInfoRepository.FindAsync(user)).DeviceToken;
                        User sender = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotification.AuthorId));
                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                        {
                            Token = token,

                            Data = new Dictionary<string, string>()
                            {
                                {"notification", JsonConvert.SerializeObject(notiViewModel)}
                            },
                            Notification = new Notification()
                            {
                                Title = "Quản trị viên CoStudy",
                                Body = notiViewModel.Content,
                                ImageUrl = configuration["AdminAvatar"]
                            }
                        };
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }


        public async Task<string> SendNotification()
        {
            FirebaseAdmin.Messaging.Message message = new FirebaseAdmin.Messaging.Message()
            {
                Token =
                    "d2Yvrq1sT8iZyZvgKtaE2Z:APA91bGuJp5Cyt2J1WuPB-GhEA3sR1eG-CwnIFJn0E2BguDjenY4TDy9gyY0phVJ8VqCZL_3Z56gaG-tBoIXjPZGxmHRpcT_CsQbNi-OxJ-XCJDxQx9LFWGgvnVPxyNqPoTm50GzNmwz",

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

        public async Task AddToGroup(AddUserToGroupRequest request)
        {
            if (request.UserIds == null)
            {
                request.UserIds = new List<string>();
            }

            FilterDefinition<ClientGroup> builder = Builders<ClientGroup>.Filter.Eq("name", request.GroupName);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(builder);

            if (clientGroup != null)
            {
                foreach (string userId in request.UserIds)
                {
                    string _userId = clientGroup.UserIds.FirstOrDefault(x => x == userId);

                    if (string.IsNullOrEmpty(_userId))
                    {
                        clientGroup.UserIds.Add(userId);
                    }
                }

                await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
            }
            else
            {
                ClientGroup group = new ClientGroup()
                {
                    Name = request.GroupName,
                    GroupType = request.Type
                };
                group.UserIds.AddRange(request.UserIds.Distinct());

                await clientGroupRepository.AddAsync(group);
            }

            ;
        }

        public async Task RemoveFromGroup(RemoveFromGroupRequest request)
        {
            if (request.UserIds == null)
            {
                request.UserIds = new List<string>();
            }

            FilterDefinition<ClientGroup> builder = Builders<ClientGroup>.Filter.Eq("name", request.GroupName);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(builder);

            if (clientGroup != null)
            {
                foreach (string userId in request.UserIds)
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

        public async Task PushNotifyDetail(string clientGroupName, NotificationDetail notificationDetail)
        {
            FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", clientGroupName);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            foreach (string receiverId in clientGroup.UserIds)
            {
                var existNotificationDetailBuilder = Builders<NotificationDetail>.Filter;
                var existNotificationDetailFilter =
                    existNotificationDetailBuilder.Eq("creator_id", notificationDetail.CreatorId)
                    & existNotificationDetailBuilder.Eq("receiver_id", receiverId)
                    & existNotificationDetailBuilder.Eq("notification_object_id",
                        notificationDetail.NotificationObjectId);

                var existNotificationDetail =
                    await notificationDetailRepository.FindAsync(existNotificationDetailFilter);

                var finalNotificationDetail = new NotificationDetail();

                if (existNotificationDetail != null)
                {
                    finalNotificationDetail = existNotificationDetail;
                    finalNotificationDetail.ModifiedDate = DateTime.Now;
                    finalNotificationDetail.IsDeleted = false;
                    await notificationDetailRepository.UpdateAsync(finalNotificationDetail, finalNotificationDetail.Id);
                }
                else
                {
                    finalNotificationDetail.CreatorId = notificationDetail.CreatorId;
                    finalNotificationDetail.ReceiverId = receiverId;
                    finalNotificationDetail.NotificationObjectId = notificationDetail.NotificationObjectId;
                    await notificationDetailRepository.AddAsync(finalNotificationDetail);
                }

                ;

                var notificationObject =
                    await notificationObjectRepository.GetByIdAsync(
                        ObjectId.Parse(finalNotificationDetail.NotificationObjectId));

                var owner = await userRepository.GetByIdAsync(ObjectId.Parse(notificationObject.OwnerId));

                var receiver = await userRepository.GetByIdAsync(ObjectId.Parse(receiverId));

                var creator = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.CreatorId));

                var notificationTypeFilter =
                    Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                var notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

                string notifyContent = string.Empty;

                if (owner.OId == creator.OId)
                {
                    if (receiver.OId == owner.OId)
                    {
                        notifyContent = $"Bạn {notificationType.ContentTemplate} chính mình";
                    }
                    else if (receiver.OId != owner.OId)
                    {
                        notifyContent = $"{creator.LastName} {notificationType.ContentTemplate} họ";
                    }
                }
                else if (owner.OId != creator.OId)
                {
                    if (receiver.OId != owner.OId)
                    {
                        notifyContent = $"{creator.LastName} {notificationType.ContentTemplate} {owner.LastName}";
                    }
                    else if (receiver.OId != creator.OId)
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

                switch (notificationType.Code)
                {
                    case "ADD_POST_NOTIFY":
                    case "UPVOTE_POST_NOTIFY":
                    case "DOWNVOTE_POST_NOTIFY":
                        notificationToPush.NotificationType = PushedNotificationType.Post;
                        break;

                    case "UPVOTE_COMMENT_NOTIFY":
                    case "DOWNVOTE_COMMENT_NOTIFY":
                    case "COMMENT_NOTIFY":
                        notificationToPush.NotificationType = PushedNotificationType.Comment;
                        break;

                    case "UPVOTE_REPLY_NOTIFY":
                    case "DOWNVOTE_REPLY_NOTIFY":
                    case "REPLY_COMMENT_NOTIFY":
                        notificationToPush.NotificationType = PushedNotificationType.Reply;
                        break;

                    case "FOLLOW_NOTIFY":
                        notificationToPush.NotificationType = PushedNotificationType.User;
                        break;

                    default:
                        notificationToPush.NotificationType = PushedNotificationType.Other;
                        break;
                }

                try
                {
                    if (!(owner.OId == creator.OId && receiver.OId == owner.OId))
                    {
                        var userFilter = Builders<FcmInfo>.Filter.Eq("user_id", receiverId);
                        string token = (await fcmInfoRepository.FindAsync(userFilter)).DeviceToken;

                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                        {
                            Token = token,
                            Data = new Dictionary<string, string>()
                            {
                                {"notification", JsonConvert.SerializeObject(notificationToPush)}
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

        public async Task PushNotifyReport(string userId, NotificationDetail notificationDetail)
        {
            FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", userId);
            ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            foreach (string id in clientGroup.UserIds)
            {
                NotificationDetail finalNotificationDetail = new NotificationDetail()
                {
                    CreatorId = notificationDetail.CreatorId,
                    ReceiverId = userId,
                    NotificationObjectId = notificationDetail.NotificationObjectId
                };
                await notificationDetailRepository.AddAsync(finalNotificationDetail);

                NotificationObject notificationObject =
                    await notificationObjectRepository.GetByIdAsync(
                        ObjectId.Parse(finalNotificationDetail.NotificationObjectId));

                FilterDefinition<NotificationType> notificationTypeFilter =
                    Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                NotificationType notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

                User creator = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.CreatorId));

                PushedNotificationViewModel notificationToPush = new PushedNotificationViewModel()
                {
                    AuthorName = $"{creator.FirstName} {creator.LastName}",
                    AuthorAvatar = creator.AvatarHash,
                    AuthorId = creator.OId,
                    Content = $" Một người {notificationType.ContentTemplate} bạn.",
                    CreatedDate = DateTime.Now,
                    IsRead = false,
                    ModifiedDate = DateTime.Now,
                    ObjectId = notificationObject.ObjectId,
                    OId = notificationObject.OId,
                    OwnerId = userId,
                    Status = ItemStatus.Active
                };
                //Push notification
                try
                {
                    FilterDefinition<FcmInfo> userFilter = Builders<FcmInfo>.Filter.Eq("user_id", userId);
                    string token = (await fcmInfoRepository.FindAsync(userFilter)).DeviceToken;

                    FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = token,
                        Data = new Dictionary<string, string>()
                        {
                            {"notification", JsonConvert.SerializeObject(notificationToPush)}
                        },
                        Notification = new Notification()
                        {
                            Title = creator.LastName,
                            Body = notificationToPush.Content,
                            ImageUrl = creator.AvatarHash
                        }
                    };
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                }
                catch (Exception)
                {
                    // do nothing 
                }
            }
        }

        public async Task PushNotifyApproveReport(string userId, NotificationDetail notificationDetail)
        {
            try
            {
                FilterDefinition<ClientGroup> finder = Builders<ClientGroup>.Filter.Eq("name", userId);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(finder);

                User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

                foreach (string id in clientGroup.UserIds)
                {
                    NotificationDetail finalNotificationDetail = new NotificationDetail()
                    {
                        CreatorId = notificationDetail.CreatorId,
                        ReceiverId = userId,
                        NotificationObjectId = notificationDetail.NotificationObjectId
                    };
                    await notificationDetailRepository.AddAsync(finalNotificationDetail);

                    NotificationObject notificationObject =
                        await notificationObjectRepository.GetByIdAsync(
                            ObjectId.Parse(finalNotificationDetail.NotificationObjectId));

                    FilterDefinition<NotificationType> notificationTypeFilter =
                        Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                    NotificationType notificationType =
                        await notificationTypeRepository.FindAsync(notificationTypeFilter);

                    User creator = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.CreatorId));

                    PushedNotificationViewModel notificationToPush = new PushedNotificationViewModel()
                    {
                        AuthorName = $"{creator.FirstName} {creator.LastName}",
                        AuthorAvatar = creator.AvatarHash,
                        AuthorId = creator.OId,
                        Content = $"{notificationType.ContentTemplate}.",
                        CreatedDate = DateTime.Now,
                        IsRead = false,
                        ModifiedDate = DateTime.Now,
                        ObjectId = notificationObject.ObjectId,
                        OId = notificationObject.OId,
                        OwnerId = userId,
                        Status = ItemStatus.Active
                    };
                    //Push notification
                    try
                    {
                        FilterDefinition<FcmInfo> userFilter = Builders<FcmInfo>.Filter.Eq("user_id", userId);
                        string token = (await fcmInfoRepository.FindAsync(userFilter)).DeviceToken;

                        FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                        {
                            Token = token,
                            Data = new Dictionary<string, string>()
                            {
                                {"notification", JsonConvert.SerializeObject(notificationToPush)}
                            },
                            Notification = new Notification()
                            {
                                Title = creator.LastName,
                                Body = notificationToPush.Content,
                                ImageUrl = creator.AvatarHash
                            }
                        };
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                    }
                    catch (Exception)
                    {
                        // do nothing 
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        public async Task PushNotifyPostMatch(string userId, NotificationDetail notificationDetail)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            await AddToGroup(
                new AddUserToGroupRequest
                {
                    GroupName = userId,
                    Type = Feature.GetTypeName(currentUser),
                    UserIds = new List<string> { userId }
                }
            );
            var finder = Builders<ClientGroup>.Filter.Eq("name", userId);
            var clientGroup = await clientGroupRepository.FindAsync(finder);

            foreach (string id in clientGroup.UserIds)
            {
                NotificationDetail finalNotificationDetail = new NotificationDetail()
                {
                    CreatorId = notificationDetail.CreatorId,
                    ReceiverId = userId,
                    NotificationObjectId = notificationDetail.NotificationObjectId
                };
                await notificationDetailRepository.AddAsync(finalNotificationDetail);

                NotificationObject notificationObject =
                    await notificationObjectRepository.GetByIdAsync(
                        ObjectId.Parse(finalNotificationDetail.NotificationObjectId));

                FilterDefinition<NotificationType> notificationTypeFilter =
                    Builders<NotificationType>.Filter.Eq("code", notificationObject.NotificationType);

                NotificationType notificationType = await notificationTypeRepository.FindAsync(notificationTypeFilter);

                User creator = await userRepository.GetByIdAsync(ObjectId.Parse(finalNotificationDetail.CreatorId));

                PushedNotificationViewModel notificationToPush = new PushedNotificationViewModel()
                {
                    AuthorName = $"{creator.FirstName} {creator.LastName}",
                    AuthorAvatar = creator.AvatarHash,
                    AuthorId = creator.OId,
                    Content = $"{notificationType.ContentTemplate}.",
                    CreatedDate = DateTime.Now,
                    IsRead = false,
                    ModifiedDate = DateTime.Now,
                    ObjectId = notificationObject.ObjectId,
                    OId = notificationObject.OId,
                    OwnerId = userId,
                    Status = ItemStatus.Active
                };
                //Push notification
                try
                {
                    FilterDefinition<FcmInfo> userFilter = Builders<FcmInfo>.Filter.Eq("user_id", userId);
                    string token = (await fcmInfoRepository.FindAsync(userFilter)).DeviceToken;

                    FirebaseAdmin.Messaging.Message mes = new FirebaseAdmin.Messaging.Message()
                    {
                        Token = token,
                        Data = new Dictionary<string, string>()
                        {
                            {"notification", JsonConvert.SerializeObject(notificationToPush)}
                        },
                        Notification = new Notification()
                        {
                            Title = creator.LastName,
                            Body = notificationToPush.Content,
                            ImageUrl = creator.AvatarHash
                        }
                    };
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(mes).ConfigureAwait(true);
                }
                catch (Exception)
                {
                    // do nothing 
                }
            }
        }
    }
}
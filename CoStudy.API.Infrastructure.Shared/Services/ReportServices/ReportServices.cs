using AutoMapper;
using Common.Constant;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class ReportServices : IReportServices
    {
        IReportRepository reportRepository;

        IMapper mapper;

        IUserRepository userRepository;

        IHttpContextAccessor httpContextAccessor;

        IPostRepository postRepository;

        ICommentRepository commentRepository;

        IReplyCommentRepository replyCommentRepository;

        INotificationObjectRepository notificationObjectRepository;

        IFcmRepository fcmRepository;

        public ReportServices(IReportRepository reportRepository,
        IMapper mapper, IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IReplyCommentRepository replyCommentRepository, INotificationObjectRepository notificationObjectRepository, IFcmRepository fcmRepository)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.notificationObjectRepository = notificationObjectRepository;
            this.fcmRepository = fcmRepository;
        }


        public async Task<ReportViewModel> Add(Report entity)
        {
            Report data = new Report()
            {
                CreatedDate = DateTime.Now,
                AuthorId = entity.AuthorId,
                IsApproved = false,
                ModifiedDate = DateTime.Now,
                Reason = entity.Reason,
                ObjectId = entity.ObjectId,
            };
            await reportRepository.AddAsync(data);
            return mapper.Map<ReportViewModel>(data);
        }

        public async Task<ReportViewModel> PostReport(CreatePostReportRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            Post post = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            Report data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.PostId,
                ObjectType = Feature.GetTypeName(post)
            };

            await fcmRepository.AddToGroup(
                new AddUserToGroupRequest
                {
                    GroupName = post.AuthorId,
                    Type = Feature.GetTypeName(currentUser),
                    UserIds = new List<string> { post.AuthorId }
                }
            );

            await reportRepository.AddAsync(data);

            FilterDefinitionBuilder<NotificationObject> notificationObjectBuilders = Builders<NotificationObject>.Filter;

            FilterDefinition<NotificationObject> notificationObjectFilters = notificationObjectBuilders.Eq("object_id", request.PostId)
                & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "REPORT_POST_NOTIFY");

            NotificationObject existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

            string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

            if (existNotificationObject == null)
            {
                NotificationObject newNotificationObject = new NotificationObject()
                {
                    NotificationType = "REPORT_POST_NOTIFY",
                    ObjectId = request.PostId,
                    OwnerId = post.AuthorId
                };
                await notificationObjectRepository.AddAsync(newNotificationObject);
                notificationObject = newNotificationObject.OId;
            }

            NotificationDetail notificationDetail = new NotificationDetail()
            {
                CreatorId = currentUser.OId,
                NotificationObjectId = notificationObject
            };


            await fcmRepository.PushNotifyReport(post.AuthorId, notificationDetail);

            return mapper.Map<ReportViewModel>(data);

        }

        public IEnumerable<ReportViewModel> GetAll(BaseGetAllRequest request)
        {
            IQueryable<Report> data = reportRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return mapper.Map<IEnumerable<ReportViewModel>>(data);
        }

        public async Task<IEnumerable<ReportViewModel>> Approve(IEnumerable<string> ids)
        {
            List<Report> dataToApprove = new List<Report>();
            foreach (string item in ids)
            {
                Report rp = await reportRepository.GetByIdAsync(ObjectId.Parse(item));
                if (rp != null)
                {
                    dataToApprove.Add(rp);
                }
            }

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            foreach (Report item in dataToApprove)
            {
                #region Find the same object

                var reportBuilders = Builders<Report>.Filter;
                var reportFilter = reportBuilders.Eq("object_id", item.ObjectId);
                var sameObjects = (await reportRepository.FindListAsync(reportFilter)).ToList();
                sameObjects.ForEach(async x =>
                {
                    x.IsApproved = true;
                    x.ModifiedDate = DateTime.Now;
                    x.ApprovedBy = currentUser.OId;
                    x.ApproveDate = DateTime.Now;
                    await reportRepository.UpdateAsync(x, x.Id);
                });

                #endregion

                if (item.ObjectType.Contains("Post"))
                {
                    Post post = await postRepository.GetByIdAsync(ObjectId.Parse(item.ObjectId));
                    post.Status = ItemStatus.Blocked;
                    post.ModifiedDate = DateTime.Now;
                    await postRepository.UpdateAsync(post, post.Id);

                    //Notify
                    FilterDefinitionBuilder<NotificationObject> notificationObjectBuilders = Builders<NotificationObject>.Filter;

                    FilterDefinition<NotificationObject> notificationObjectFilters = notificationObjectBuilders.Eq("object_id", post.OId)
                        & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "APPROVE_POST_REPORT");

                    NotificationObject existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                    string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                    if (existNotificationObject == null)
                    {
                        NotificationObject newNotificationObject = new NotificationObject()
                        {
                            NotificationType = "APPROVE_POST_REPORT",
                            ObjectId = post.OId,
                            OwnerId = post.AuthorId
                        };
                        await notificationObjectRepository.AddAsync(newNotificationObject);
                        notificationObject = newNotificationObject.OId;
                    }

                    NotificationDetail notificationDetail = new NotificationDetail()
                    {
                        CreatorId = currentUser.OId,
                        NotificationObjectId = notificationObject
                    };

                    await fcmRepository.PushNotifyApproveReport(post.AuthorId, notificationDetail);
                }
                else
                if (item.ObjectType.Contains("ReplyComment"))
                {
                    ReplyComment replyComment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(item.ObjectId));
                    replyComment.Status = ItemStatus.Blocked;
                    replyComment.ModifiedDate = DateTime.Now;
                    await replyCommentRepository.UpdateAsync(replyComment, replyComment.Id);

                    //Notify
                    FilterDefinitionBuilder<NotificationObject> notificationObjectBuilders = Builders<NotificationObject>.Filter;

                    FilterDefinition<NotificationObject> notificationObjectFilters = notificationObjectBuilders.Eq("object_id", replyComment.OId)
                        & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "APPROVE_REPLY_REPORT");

                    NotificationObject existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                    string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                    if (existNotificationObject == null)
                    {
                        NotificationObject newNotificationObject = new NotificationObject()
                        {
                            NotificationType = "APPROVE_REPLY_REPORT",
                            ObjectId = replyComment.OId,
                            OwnerId = replyComment.AuthorId
                        };
                        await notificationObjectRepository.AddAsync(newNotificationObject);
                        notificationObject = newNotificationObject.OId;
                    }

                    NotificationDetail notificationDetail = new NotificationDetail()
                    {
                        CreatorId = currentUser.OId,
                        NotificationObjectId = notificationObject
                    };


                    await fcmRepository.PushNotifyApproveReport(replyComment.AuthorId, notificationDetail);
                }
                else
                if (item.ObjectType.Contains("Comment"))
                {
                    Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(item.ObjectId));
                    comment.Status = ItemStatus.Blocked;
                    comment.ModifiedDate = DateTime.Now;
                    await commentRepository.UpdateAsync(comment, comment.Id);

                    //Notify

                    FilterDefinitionBuilder<NotificationObject> notificationObjectBuilders = Builders<NotificationObject>.Filter;

                    FilterDefinition<NotificationObject> notificationObjectFilters = notificationObjectBuilders.Eq("object_id", comment.OId)
                        & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "APPROVE_REPLY_REPORT");

                    NotificationObject existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

                    string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

                    if (existNotificationObject == null)
                    {
                        NotificationObject newNotificationObject = new NotificationObject()
                        {
                            NotificationType = "APPROVE_REPLY_REPORT",
                            ObjectId = comment.OId,
                            OwnerId = comment.AuthorId
                        };
                        await notificationObjectRepository.AddAsync(newNotificationObject);
                        notificationObject = newNotificationObject.OId;
                    }

                    NotificationDetail notificationDetail = new NotificationDetail()
                    {
                        CreatorId = currentUser.OId,
                        NotificationObjectId = notificationObject
                    };


                    await fcmRepository.PushNotifyApproveReport(comment.AuthorId, notificationDetail);
                }
            }
            return mapper.Map<IEnumerable<ReportViewModel>>(dataToApprove);
        }

        public async Task<ReportViewModel> CommentReport(CreateCommentReportRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.CommentId));

            var data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.CommentId,
                ObjectType = Feature.GetTypeName(comment)
            };

            await fcmRepository.AddToGroup(
                new AddUserToGroupRequest
                {
                    GroupName = comment.AuthorId,
                    Type = Feature.GetTypeName(currentUser),
                    UserIds = new List<string> { comment.AuthorId }
                }
            );

            await reportRepository.AddAsync(data);

            var notificationObjectBuilders = Builders<NotificationObject>.Filter;

            var notificationObjectFilters = notificationObjectBuilders.Eq("object_id", request.CommentId)
                                            & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "REPORT_COMMENT_NOTIFY");

            var existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

            string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

            if (existNotificationObject == null)
            {
                var newNotificationObject = new NotificationObject()
                {
                    NotificationType = "REPORT_COMMENT_NOTIFY",
                    ObjectId = request.CommentId,
                    OwnerId = comment.AuthorId
                };
                await notificationObjectRepository.AddAsync(newNotificationObject);
                notificationObject = newNotificationObject.OId;
            }

            var notificationDetail = new NotificationDetail()
            {
                CreatorId = currentUser.OId,
                NotificationObjectId = notificationObject
            };

            await fcmRepository.PushNotifyReport(comment.AuthorId, notificationDetail);

            return mapper.Map<ReportViewModel>(data);
        }

        public async Task<ReportViewModel> ReplyReport(CreateReplyReportRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            ReplyComment comment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(request.ReplyId));

            Report data = new Report()
            {
                AuthorId = currentUser.OId,
                Reason = request.Reason.ToList(),
                ObjectId = request.ReplyId,
                ObjectType = Feature.GetTypeName(comment)
            };

            await fcmRepository.AddToGroup(
                new AddUserToGroupRequest
                {
                    GroupName = comment.AuthorId,
                    Type = Feature.GetTypeName(currentUser),
                    UserIds = new List<string> { comment.AuthorId }
                }
            );

            await reportRepository.AddAsync(data);

            FilterDefinitionBuilder<NotificationObject> notificationObjectBuilders = Builders<NotificationObject>.Filter;

            FilterDefinition<NotificationObject> notificationObjectFilters = notificationObjectBuilders.Eq("object_id", request.ReplyId)
                & notificationObjectBuilders.Eq(NotificationConstant.NotificationType, "REPORT_REPLY_NOTIFY");

            NotificationObject existNotificationObject = await notificationObjectRepository.FindAsync(notificationObjectFilters);

            string notificationObject = existNotificationObject != null ? existNotificationObject.OId : string.Empty;

            if (existNotificationObject == null)
            {
                NotificationObject newNotificationObject = new NotificationObject()
                {
                    NotificationType = "REPORT_REPLY_NOTIFY",
                    ObjectId = request.ReplyId,
                    OwnerId = comment.AuthorId
                };
                await notificationObjectRepository.AddAsync(newNotificationObject);
                notificationObject = newNotificationObject.OId;
            }

            NotificationDetail notificationDetail = new NotificationDetail()
            {
                CreatorId = currentUser.OId,
                NotificationObjectId = notificationObject
            };

            await fcmRepository.PushNotifyReport(comment.AuthorId, notificationDetail);

            return mapper.Map<ReportViewModel>(data);
        }

        public async Task<ReportViewModel> GetReportById(string id)
        {
            var data = await reportRepository.GetByIdAsync(ObjectId.Parse(id));
            var result = mapper.Map<ReportViewModel>(data);
            return result;
        }

    }
}

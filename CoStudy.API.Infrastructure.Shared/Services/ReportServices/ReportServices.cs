using AutoMapper;
using Common.Constant;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        IConfiguration configuration;
        public ReportServices(IReportRepository reportRepository,
        IMapper mapper, IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IReplyCommentRepository replyCommentRepository, INotificationObjectRepository notificationObjectRepository, IFcmRepository fcmRepository, IConfiguration configuration)
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
            this.configuration = configuration;
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

            var notificationDetail = new Noftication()
            {
                AuthorId = currentUser.OId,
                OwnerId = currentUser.OId,
                ObjectId = post.OId,
                ObjectThumbnail = post.Title
            };

            //Bài viết của bạn đã bị báo cáo. 
            await fcmRepository.PushNotify(post.AuthorId, notificationDetail, NotificationContent.PostReportNotification, "Bài viết của bạn đã bị báo cáo. ");
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
                    post.Status = ItemStatus.Deleted;
                    post.ModifiedDate = DateTime.Now;
                    await postRepository.UpdateAsync(post, post.Id);
                    var user = await userRepository.GetByIdAsync(ObjectId.Parse(post.AuthorId));
                    if (user == null)
                        throw new Exception("Không tìm thấy người dùng. ");

                    var notificationDetail = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentUser.OId,
                        ObjectId = post.OId,
                        ObjectThumbnail = post.Title
                    };

                    await fcmRepository.PushNotify(post.AuthorId,
                       notificationDetail,
                       NotificationContent.ApprovePostReportNotification,
                       $"Bài viết của bạn đã bị xóa bởi quản trị viên. ");

                    //Báo cáo của bạn đã được duyệt. 
                    await fcmRepository.PushNotify(item.AuthorId, 
                        notificationDetail, 
                        NotificationContent.ApprovePostReportNotification, 
                        $"Báo cáo của bạn về bài viết của{user.FirstName} {user.LastName} đã được xem xét. ");

                   
                }
                else
                if (item.ObjectType.Contains("ReplyComment"))
                {
                    ReplyComment replyComment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(item.ObjectId));
                    replyComment.Status = ItemStatus.Deleted;
                    replyComment.ModifiedDate = DateTime.Now;
                    await replyCommentRepository.UpdateAsync(replyComment, replyComment.Id);

                    var user = await userRepository.GetByIdAsync(ObjectId.Parse(replyComment.AuthorId));
                    if (user == null)
                        throw new Exception("Không tìm thấy người dùng. ");

                    var notificationDetail = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentUser.OId,
                        ObjectId = replyComment.OId,
                        ObjectThumbnail = replyComment.Content
                    };

                    //Báo cáo của bạn đã được duyệt. 
                    await fcmRepository.PushNotify(item.AuthorId,
                        notificationDetail,
                        NotificationContent.ApproveReplyReportNotification,
                        $"Báo cáo của bạn về phản hồi của {user.FirstName} {user.LastName} đã được xem xét. ");

                    await fcmRepository.PushNotify(replyComment.AuthorId,
                        notificationDetail,
                        NotificationContent.ApproveReplyReportNotification,
                        $"Phản hồi của bạn đã bị xóa bởi quản trị viên. ");
                }
                else
                if (item.ObjectType.Contains("Comment"))
                {
                    Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(item.ObjectId));
                    comment.Status = ItemStatus.Deleted;
                    comment.ModifiedDate = DateTime.Now;
                    await commentRepository.UpdateAsync(comment, comment.Id);

                    var user = await userRepository.GetByIdAsync(ObjectId.Parse(comment.AuthorId));
                    if (user == null)
                        throw new Exception("Không tìm thấy người dùng. ");

                    var notificationDetail = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentUser.OId,
                        ObjectId = comment.OId,
                        ObjectThumbnail = comment.Content
                    };

                    //Báo cáo của bạn đã được duyệt. 
                    await fcmRepository.PushNotify(item.AuthorId,
                        notificationDetail,
                        NotificationContent.ApproveCommentReportNotification,
                        $"Báo cáo của bạn về bình luận của {user.FirstName} {user.LastName} đã được xem xét.");

                    await fcmRepository.PushNotify(comment.AuthorId,
                       notificationDetail,
                       NotificationContent.ApproveCommentReportNotification,
                       $"Bình luận của bạn đã bị xóa bởi quản trị viên. ");
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

            var notificationDetail = new Noftication()
            {
                AuthorId = currentUser.OId,
                OwnerId = currentUser.OId,
                ObjectId = comment.OId,
                ObjectThumbnail = comment.Content
            };

            //Bài viết của bạn đã bị báo cáo. 
            await fcmRepository.PushNotify(comment.AuthorId, notificationDetail, NotificationContent.CommentReportNotification, "Bình luận của bạn đã bị báo cáo. ");

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

            var notificationDetail = new Noftication()
            {
                AuthorId = currentUser.OId,
                OwnerId = currentUser.OId,
                ObjectId = comment.OId,
                ObjectThumbnail = comment.Content
            };

            //Bài viết của bạn đã bị báo cáo. 
            await fcmRepository.PushNotify(comment.AuthorId, notificationDetail, NotificationContent.CommentReportNotification, "Phản hồi của bạn đã bị báo cáo. ");


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

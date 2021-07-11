using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Hangfire
{
    public class HangfireService : IHangfireService
    {
        IPostService postService;
        IPostRepository postRepository;
        ILoggingRepository loggingRepository;
        IFcmRepository fcmRepository;
        IReportRepository reportRepository;
        ICommentRepository commentRepository;
        IReplyCommentRepository replyCommentRepository;
        ICommentService commentService;
        IReportServices reportServices;
        public HangfireService(IPostService postService,
            IPostRepository postRepository,
            ILoggingRepository loggingRepository,
            IFcmRepository fcmRepository,
            IReportRepository reportRepository,
            ICommentRepository commentRepository,
            ICommentService commentService,
            IReplyCommentRepository replyCommentRepository, IReportServices reportServices)
        {
            this.postService = postService;
            this.postRepository = postRepository;
            this.loggingRepository = loggingRepository;
            this.fcmRepository = fcmRepository;
            this.reportRepository = reportRepository;
            this.commentRepository = commentRepository;
            this.commentService = commentService;
            this.replyCommentRepository = replyCommentRepository;
            this.reportServices = reportServices;
        }

        public string RemoveViolencePost()
        {
            var posts = postRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
            var deletedPost = 0;
            foreach (var post in posts)
            {
                try
                {
                    var isViolence = postService.IsViolencePost(post.OId);
                    if (isViolence == true)
                    {
                        var notificationDetail = new Noftication()
                        {
                            AuthorId = "60b5f2623d52db390d464e3e",
                            OwnerId = "60b5f2623d52db390d464e3e",
                            ObjectId = post.OId,
                            ObjectThumbnail = post.Title
                        };

                        fcmRepository.PushNotify(post.AuthorId,
                         notificationDetail,
                         NotificationContent.ApprovePostReportNotification,
                         $"Bài viết của bạn đã bị xóa vì vi phạm quy định của chúng tôi. ").ConfigureAwait(true);

                        deletedPost++;
                        post.Status = ItemStatus.Blocked;
                        postRepository.Update(post, post.Id);
                      
                        var report = new Report()
                        {
                            ObjectId = post.OId,
                            ObjectType = Feature.GetTypeName(post),
                            CreatedDate = DateTime.Now,
                            IsApproved = true,
                            ApprovedBy = "60b5f2623d52db390d464e3e",
                            ApproveDate = DateTime.Now,
                            AuthorId = post.AuthorId,
                            Reason = new System.Collections.Generic.List<string>() { "606bc1ddd01f5aa1a3e282f5" }
                        };
                        reportRepository.Add(report);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return $"Task success with {deletedPost} post deleted at {DateTime.Now.ToUniversalTime()}";
        }

        public string RemoveViolenceComment()
        {
            var comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
            var deletedComment = 0;
            foreach (var comment in comments)
            {
                try
                {
                    var isViolence = commentService.IsViolenceComment(comment.OId);
                    if (isViolence == true)
                    {
                        var notificationDetail = new Noftication()
                        {
                            AuthorId = "60b5f2623d52db390d464e3e",
                            OwnerId = "60b5f2623d52db390d464e3e",
                            ObjectId = comment.OId,
                            ObjectThumbnail = comment.Content
                        };
                        fcmRepository.PushNotify(comment.AuthorId,
                           notificationDetail,
                           NotificationContent.ApproveCommentReportNotification,
                           $"Bình luận của bạn đã bị xóa bởi vì vi phạm quy định của chúng tôi. ").ConfigureAwait(true);

                        deletedComment++;
                        comment.Status = ItemStatus.Blocked;
                        commentRepository.Update(comment, comment.Id);

                       
                        var report = new Report()
                        {
                            ObjectId = comment.OId,
                            ObjectType = Feature.GetTypeName(comment),
                            CreatedDate = DateTime.Now,
                            IsApproved = true,
                            ApprovedBy = "60b5f2623d52db390d464e3e",
                            ApproveDate = DateTime.Now,
                            AuthorId = comment.AuthorId,
                            Reason = new System.Collections.Generic.List<string>() { "606bc1ddd01f5aa1a3e282f5" }
                        };
                        reportRepository.Add(report);

                        
                    }
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }

            return $"Task complete with {deletedComment} comments deleted at {DateTime.Now.ToUniversalTime()}";
        }

        public string RemoveViolenceReply()
        {
            var comments = replyCommentRepository.GetAll().Where(x => x.Status == ItemStatus.Active);
            var deletedComment = 0;
            foreach (var comment in comments)
            {
                try
                {
                    var isViolence = commentService.IsViolenceReply(comment.OId);
                    if (isViolence == true)
                    {
                        var notificationDetail = new Noftication()
                        {
                            AuthorId = "60b5f2623d52db390d464e3e",
                            OwnerId = "60b5f2623d52db390d464e3e",
                            ObjectId = comment.OId,
                            ObjectThumbnail = comment.Content
                        };

                        fcmRepository.PushNotify(comment.AuthorId,
                        notificationDetail,
                        NotificationContent.ApproveReplyReportNotification,
                        $"Phản hồi của bạn đã bị xóa bởi vì vi phạm quy định của chúng tôi. ").ConfigureAwait(true);

                        deletedComment++;
                        comment.Status = ItemStatus.Blocked;
                        replyCommentRepository.Update(comment, comment.Id);

                        var report = new Report()
                        {
                            ObjectId = comment.OId,
                            ObjectType = Feature.GetTypeName(comment),
                            CreatedDate = DateTime.Now,
                            IsApproved = true,
                            ApprovedBy = "60b5f2623d52db390d464e3e",
                            ApproveDate = DateTime.Now,
                            AuthorId = comment.AuthorId,
                            Reason = new System.Collections.Generic.List<string>() { "606bc1ddd01f5aa1a3e282f5" }
                        };

                        reportRepository.Add(report);
                     
                    }
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }

            return $"Task complete with {deletedComment} reply comments deleted at {DateTime.Now.ToUniversalTime()}";
        }

    }
}

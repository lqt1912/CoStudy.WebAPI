using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Application.Utitlities;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using static Common.Constants;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class CommentService : ICommentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IReplyCommentRepository replyCommentRepository;
        private readonly IDownVoteRepository downVoteRepository;
        private readonly IUpVoteRepository upVoteRepository;
        private readonly IFcmRepository fcmRepository;
        private readonly IMapper mapper;
        private readonly INotificationObjectRepository notificationObjectRepository;
        private readonly IObjectLevelRepository objectLevelRepository;
        private readonly ILevelRepository levelRepository;
        private readonly IUserService userService;
        private readonly IViolenceWordRepository violenceWordRepository;
        public CommentService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReplyCommentRepository replyCommentRepository,
            IDownVoteRepository downVoteRepository,
            IUpVoteRepository upVoteRepository,
            IFcmRepository fcmRepository,
            IMapper mapper,
            INotificationObjectRepository notificationObjectRepository,
            IObjectLevelRepository objectLevelRepository,
            ILevelRepository levelRepository, 
            IUserService userService, 
            IViolenceWordRepository violenceWordRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.downVoteRepository = downVoteRepository;
            this.upVoteRepository = upVoteRepository;
            this.fcmRepository = fcmRepository;
            this.mapper = mapper;
            this.notificationObjectRepository = notificationObjectRepository;
            this.objectLevelRepository = objectLevelRepository;
            this.levelRepository = levelRepository;
            this.userService = userService;
            this.violenceWordRepository = violenceWordRepository;
        }

        public async Task<CommentViewModel> AddComment(AddCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            if (currentPost is { Status: ItemStatus.Active })
            {
                var author = await userRepository.GetByIdAsync(ObjectId.Parse(currentPost.AuthorId));
                var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());

                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                await commentRepository.AddAsync(comment);

                await fcmRepository.AddToGroup(
                    new AddUserToGroupRequest
                    {
                        GroupName = currentPost.OId,
                        Type = Feature.GetTypeName(currentPost),
                        UserIds = new List<string> { currentUser.OId }
                    }
                );

                await fcmRepository.AddToGroup(
                    new AddUserToGroupRequest()
                    {
                        GroupName = comment.OId,
                        Type = Feature.GetTypeName(comment),
                        UserIds = new List<string>() { currentUser.OId }
                    }
                    );

                //Wait for saving
                Thread.Sleep(50);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = author.OId,
                    ObjectId = currentPost.OId,
                    ObjectThumbnail = currentPost.Title
                };

                await fcmRepository.PushNotify(currentPost.OId, notificationDetail, NotificationContent.CommentNotification);

                await userService.AddPoint(currentUser.OId, currentPost.OId, null);

                //Update again
                var result = mapper.Map<CommentViewModel>(comment);
                result.AuthorField = await GetMostMatchFieldOfUser(currentUser, currentPost);
                return result;
            }

            throw new Exception("Post đã bị xóa");
        }

        public async Task<string> DeleteComment(string commentId)
        {
            var currentComment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            if (currentComment != null)
            {
                currentComment.Status = ItemStatus.Deleted;
                await commentRepository.UpdateAsync(currentComment, currentComment.Id);

                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(currentComment.PostId));
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                return "Xóa bình luận thành công";
            }

            throw new Exception("Comment không tồn tại hoặc đã bị xóa");
        }

        public async Task<string> DeleteReply(string replyId)
        {
            var currentReply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(replyId));
            if (currentReply != null)
            {
                currentReply.Status = ItemStatus.Deleted;
                await replyCommentRepository.UpdateAsync(currentReply, currentReply.Id);
                return "Xóa câu trả lời thành công";
            }

            throw new Exception("Câu rả lời không tồn tại hoặc đã bị xóa");
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentByPostId(CommentFilterRequest request)
        {
            if (string.IsNullOrEmpty(request.PostId)) throw new Exception("Không tồn tại bài post");

            var builder = Builders<Comment>.Filter;
            var filter = builder.Eq("post_id", request.PostId) & builder.Eq("status", ItemStatus.Active);

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost == null) return new List<CommentViewModel>();

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var comments = await commentRepository.FindListAsync(filter);
            if (comments != null)
            {
                var cmts = mapper.Map<IEnumerable<CommentViewModel>>(comments.AsEnumerable());

                if (request.Skip.HasValue && request.Count.HasValue)
                    cmts = cmts.Skip(request.Skip.Value).Take(request.Count.Value);

                if (request.Filter.HasValue && request.ArrangeType.HasValue)
                    switch (request.Filter.Value)
                    {
                        case CommentFilterType.CreatedDate:
                            {
                                cmts = request.ArrangeType.Value == ArrangeType.Ascending ? cmts.OrderBy(x => x.CreatedDate) : cmts.OrderByDescending(x => x.CreatedDate);

                                break;
                            }
                        case CommentFilterType.Upvote:
                            {
                                cmts = request.ArrangeType.Value == ArrangeType.Ascending ? cmts.OrderBy(x => x.UpvoteCount) : cmts.OrderByDescending(x => x.UpvoteCount);

                                break;
                            }
                        default:
                            cmts = cmts.OrderBy(x => x.CreatedDate);
                            break;
                    }

                if (!string.IsNullOrEmpty(request.Keyword)) cmts = cmts.Where(x => x.Content.Contains(request.Keyword));

                foreach (var item in cmts)
                {
                    var builderUpvote = Builders<UpVote>.Filter;
                    var finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) &
                                       builderUpvote.Eq("upvote_by", currentUser.OId);
                    var upvote = await upVoteRepository.FindAsync(finderUpvote);
                    if (upvote != null) item.IsVoteByCurrent = true;

                    var builderDownvote = Builders<DownVote>.Filter;
                    var finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) &
                                         builderDownvote.Eq("downvote_by", currentUser.OId);
                    var downvote = await downVoteRepository.FindAsync(finderDownvote);
                    if (downvote != null) item.IsDownVoteByCurrent = true;
                }
                foreach (var commentViewModel in cmts)
                {
                    var author = await userRepository.GetByIdAsync(ObjectId.Parse(commentViewModel.AuthorId));
                    commentViewModel.AuthorField = await GetMostMatchFieldOfUser(author, currentPost);
                }
                return cmts;
            }

            throw new Exception("Post không tồn tại hoặc đã bị xóa");
        }

        public async Task<IEnumerable<ReplyCommentViewModel>> GetReplyCommentByCommentId(string commentId, int skip,
            int count)
        {
            var builder = Builders<ReplyComment>.Filter;
            var filter = builder.Eq("parent_id", commentId) & builder.Eq("status", ItemStatus.Active);
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comments = mapper.Map<List<ReplyCommentViewModel>>(await replyCommentRepository.FindListAsync(filter));
            if (comments == null) throw new Exception("Bình luận không tồn tại hoặc đã bị xóa");

            var cmts = comments.Skip(skip).Take(count);

            foreach (var item in cmts)
            {
                var builderUpvote = Builders<UpVote>.Filter;
                var finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) &
                                   builderUpvote.Eq("upvote_by", currentUser.OId);
                var upvote = await upVoteRepository.FindAsync(finderUpvote);
                if (upvote != null)
                    item.IsVoteByCurrent = true;

                var builderDownvote = Builders<DownVote>.Filter;
                var finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) &
                                     builderDownvote.Eq("downvote_by", currentUser.OId);
                var downvote = await downVoteRepository.FindAsync(finderDownvote);
                if (downvote != null)
                    item.IsDownVoteByCurrent = true;
            }

            return cmts;

        }

        public async Task<CommentViewModel> GetCommentById(string commentId)
        {
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));

            if (!(comment is { Status: ItemStatus.Active })) return null;

            var result = mapper.Map<CommentViewModel>(comment);
            try
            {
                var author = await userRepository.GetByIdAsync(ObjectId.Parse(result.AuthorId));
                var post = await postRepository.GetByIdAsync(ObjectId.Parse(result.PostId));
                result.AuthorField = await GetMostMatchFieldOfUser(author, post);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        public async Task<ReplyCommentViewModel> GetReplyCommentById(string replyId)
        {
            var reply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(replyId));

            if (!(reply is { Status: ItemStatus.Active })) return null;

            var result = mapper.Map<ReplyCommentViewModel>(reply);
            try
            {
                var author = await userRepository.GetByIdAsync(ObjectId.Parse(result.AuthorId));
                var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(result.ParentId));
                var post = await postRepository.GetByIdAsync(ObjectId.Parse(comment.PostId));
                result.AuthorField = await GetMostMatchFieldOfUser(author, post);
            }
            catch (Exception)
            {
                return null;
            }
            return result;

        }
        public async Task<ReplyCommentViewModel> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                await replyCommentRepository.AddAsync(replyComment);
                await commentRepository.UpdateAsync(comment, comment.Id);


                await fcmRepository.AddToGroup(
                    new AddUserToGroupRequest
                    {
                        GroupName = replyComment.OId,
                        Type = Feature.GetTypeName(replyComment),
                        UserIds = new List<string> { currentUser.OId }
                    }
                );

                //Wait for saving
                Thread.Sleep(50);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = comment.AuthorId,
                    ObjectId = comment.OId,
                    ObjectThumbnail = comment.Content
                };

                await fcmRepository.PushNotify(comment.OId, notificationDetail, NotificationContent.ReplyCommentNotification);

                await userService.AddPoint(currentUser.OId, comment.PostId, null);

                return mapper.Map<ReplyCommentViewModel>(replyComment);
            }

            throw new Exception("Bình luận đã bị xóa");
        }

        public async Task<string> UpvoteComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));

            var builder = Builders<DownVote>.Filter;

            var downvoteFinder = builder.Eq("object_vote_id", commentId) & builder.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);
            if (existDownvote != null) await downVoteRepository.DeleteAsync(existDownvote.Id);

            var upvotebuilder = Builders<UpVote>.Filter;
            var upvoteFinder = upvotebuilder.Eq("object_vote_id", commentId) &
                               upvotebuilder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(upvoteFinder);

            if (existUpvote == null)
            {
                var upvote = new UpVote
                {
                    UpVoteBy = currentUser.OId,
                    ObjectVoteId = commentId
                };

                await upVoteRepository.AddAsync(upvote);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = comment.AuthorId,
                    ObjectId = comment.OId,
                    ObjectThumbnail = comment.Content
                };

                await fcmRepository.PushNotify(comment.OId, notificationDetail, NotificationContent.UpvoteCommentNotification);

                await userService.AddPoint(currentUser.OId, comment.PostId, PointAdded.Upvote);

                return "Upvote thành công";
            }

            return "Bạn đã upvote rồi";
        }

        public async Task<string> DownvoteComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            var builder = Builders<UpVote>.Filter;
            var finder = builder.Eq("object_vote_id", commentId) & builder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(finder);

            if (existUpvote != null) await upVoteRepository.DeleteAsync(existUpvote.Id);

            var builderDownVote = Builders<DownVote>.Filter;

            var downvoteFinder = builderDownVote.Eq("object_vote_id", commentId) &
                                 builderDownVote.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);

            if (existDownvote == null)
            {
                var downVote = new DownVote
                {
                    DownVoteBy = currentUser.OId,
                    ObjectVoteId = commentId
                };

                await downVoteRepository.AddAsync(downVote);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = comment.AuthorId,
                    ObjectId = comment.OId,
                    ObjectThumbnail = comment.Content
                };

                await fcmRepository.PushNotify(comment.OId, notificationDetail, NotificationContent.DownvoteCommentNotification);


                await userService.AddPoint(currentUser.OId, comment.PostId, PointAdded.Downvote);
                return "Downvote thành công";
            }

            return "Bạn đã downvote rồi";
        }

        public async Task<string> UpvoteReplyComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(commentId));

            var builder = Builders<DownVote>.Filter;

            var downvoteFinder = builder.Eq("object_vote_id", commentId) & builder.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);
            if (existDownvote != null) await downVoteRepository.DeleteAsync(existDownvote.Id);

            var upvotebuilder = Builders<UpVote>.Filter;
            var upvoteFinder = upvotebuilder.Eq("object_vote_id", commentId) &
                               upvotebuilder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(upvoteFinder);

            if (existUpvote == null)
            {
                var upvote = new UpVote
                {
                    UpVoteBy = currentUser.OId,
                    ObjectVoteId = commentId
                };

                await upVoteRepository.AddAsync(upvote);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = comment.AuthorId,
                    ObjectId = comment.OId,
                    ObjectThumbnail = comment.Content
                };

                await fcmRepository.PushNotify(comment.OId, notificationDetail, NotificationContent.UpvoteReplyNotification);

                return "Upvote thành công";
            }

            return "Bạn đã upvote rồi";
        }

        public async Task<string> DownvoteReplyComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var comment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            var builder = Builders<UpVote>.Filter;
            var finder = builder.Eq("object_vote_id", commentId) & builder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(finder);

            if (existUpvote != null) await upVoteRepository.DeleteAsync(existUpvote.Id);

            var builderDownVote = Builders<DownVote>.Filter;

            var downvoteFinder = builderDownVote.Eq("object_vote_id", commentId) &
                                 builderDownVote.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);

            if (existDownvote == null)
            {
                var downVote = new DownVote
                {
                    DownVoteBy = currentUser.OId,
                    ObjectVoteId = commentId
                };

                await downVoteRepository.AddAsync(downVote);

                var notificationDetail = new Noftication()
                {
                    AuthorId = currentUser.OId,
                    OwnerId = comment.AuthorId,
                    ObjectId = comment.OId,
                    ObjectThumbnail = comment.Content
                };

                await fcmRepository.PushNotify(comment.OId, notificationDetail, NotificationContent.DownvoteReplyNotification);

                return "Downvote thành công";
            }

            return "Bạn đã downvote rồi";
        }


        public async Task<CommentViewModel> UpdateComment(UpdateCommentRequest request)
        {
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (comment == null || comment.Status != ItemStatus.Active) throw new Exception("Không tìm thấy bình luận");

            comment.Content = request.Content;
            comment.Image = request.Image;
            comment.ModifiedDate = DateTime.Now;
            comment.IsEdited = true;
            await commentRepository.UpdateAsync(comment, comment.Id);
            return mapper.Map<CommentViewModel>(comment);
        }

        public async Task<ReplyCommentViewModel> UpdateReply(UpdateReplyRequest request)
        {
            var reply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (reply == null || reply.Status != ItemStatus.Active) throw new Exception("Không tìm thấy câu trả lời");

            reply.Content = request.Content;
            reply.IsEdited = true;
            reply.ModifiedDate = DateTime.Now;
            await replyCommentRepository.UpdateAsync(reply, reply.Id);
            return mapper.Map<ReplyCommentViewModel>(reply);
        }

        public async Task<ObjectLevelViewModel> GetMostMatchFieldOfUser(User user, Post post)
        {
            var _builder = Builders<ObjectLevel>.Filter;
            var userObjectLevelsFilter = _builder.Eq("object_id", user.OId);
            var userObjectLevels = await objectLevelRepository.FindListAsync(userObjectLevelsFilter);

            userObjectLevelsFilter = _builder.Eq("object_id", post.OId);
            var postObjectLevels = await objectLevelRepository.FindListAsync(userObjectLevelsFilter);
            var result = new List<ObjectLevel>();

            userObjectLevels.ForEach(x =>
               {
                   if (postObjectLevels.Any(y => y.FieldId == x.FieldId))
                   {
                       postObjectLevels.ForEach(z =>
                      {
                          if (x.FieldId == z.FieldId)
                          {
                              //var userLevel = levelRepository.GetById(ObjectId.Parse(x.LevelId));
                              //var postLevel = levelRepository.GetById(ObjectId.Parse(z.LevelId));
                              //if (userLevel.Order >= postLevel.Order)
                              result.Add(x);
                          }
                      });
                   }
               });

            if (result.Any())
                return mapper.Map<ObjectLevelViewModel>(result.ElementAt(0));
            return null;
        }

        public async Task<CommentViewModel> ModifiedCommentStatus(ModifiedCommentStatusRequest request)
        {
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.CommentId));
            if (comment == null)
                throw new Exception("Không tìm thấy bình luận. ");
            comment.Status = request.Status;
            await commentRepository.UpdateAsync(comment, comment.Id);

            var replyBuidlder = Builders<ReplyComment>.Filter;
            var reply = replyBuidlder.Eq("parent_id", comment.OId);
            var replies = await replyCommentRepository.FindListAsync(reply);
            replies.ForEach(
                async x =>
                {
                    x.Status = request.Status;
                    await replyCommentRepository.UpdateAsync(x, x.Id);
                }
            );
            return mapper.Map<CommentViewModel>(comment);
        }

        public async Task<ReplyCommentViewModel> ModifiedReplyCommentStatus(ModifiedCommentStatusRequest request)
        {
            var replyComment = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(request.CommentId));
            if (replyComment == null)
                throw new Exception("Không tìm thấy câu trả lời. ");
            replyComment.Status = request.Status;
            await replyCommentRepository.UpdateAsync(replyComment, replyComment.Id);
            return mapper.Map<ReplyCommentViewModel>(replyComment);
        }

        public bool IsViolenceComment(string commentId)
        {
            var comment = commentRepository.GetById(ObjectId.Parse(commentId));
            if(comment!=null && comment.Status == ItemStatus.Active)
            {
                var a = StringUtils.ValidateAllowString(violenceWordRepository,comment.Content);
                if (!a)
                    return true;
            }
            return false;
        }

        public bool IsViolenceReply(string replyId)
        {
            var comment = replyCommentRepository.GetById(ObjectId.Parse(replyId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                var a = StringUtils.ValidateAllowString(violenceWordRepository, comment.Content);
                if (!a)
                    return true;
            }
            return false;
        }
    }
}
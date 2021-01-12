﻿using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class CommentService : ICommentService
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserRepository userRepository;
        private IPostRepository postRepository;
        private ICommentRepository commentRepository;
        private IReplyCommentRepository replyCommentRepository;
        private IDownVoteRepository downVoteRepository;
        private IUpVoteRepository upVoteRepository;
        private IClientGroupRepository clientGroupRepository;
        private IFcmRepository fcmRepository;
        private INofticationRepository nofticationRepository;

        public CommentService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReplyCommentRepository replyCommentRepository,
            IDownVoteRepository downVoteRepository,
            IUpVoteRepository upVoteRepository,
            IClientGroupRepository clientGroupRepository,
            IFcmRepository fcmRepository,
            INofticationRepository nofticationRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.downVoteRepository = downVoteRepository;
            this.upVoteRepository = upVoteRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.fcmRepository = fcmRepository;
            this.nofticationRepository = nofticationRepository;
        }

        public async Task<AddCommentResponse> AddComment(AddCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
                comment.AuthorAvatar = currentUser.AvatarHash;
                comment.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

                currentPost.CommentCount++;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                await commentRepository.AddAsync(comment);

                #region Notification
                var filter = Builders<ClientGroup>.Filter.Eq("name", currentPost.OId);
                var clientGroup = await clientGroupRepository.FindAsync(filter);

                if (!clientGroup.UserIds.Contains(currentUser.OId))
                {
                    clientGroup.UserIds.Add(currentUser.OId);
                    await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
                }

                if (currentPost.AuthorId != currentUser.OId) //Cùng tác giả
                {
                    var notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã bình luận bài viết của {currentPost.AuthorName} ",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }
                else //Khác tác giả
                {
                    var notify = new Noftication()
                    {
                        AuthorId = currentUser.OId,
                        OwnerId = currentPost.AuthorId,
                        Content = $"{currentUser.LastName} đã bình luận bài viết của bạn",
                        AuthorName = currentUser.LastName,
                        AuthorAvatar = currentUser.AvatarHash,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    await fcmRepository.PushNotify(currentPost.OId, notify);
                    await nofticationRepository.AddAsync(notify);
                }

                //Tạo clientGroup cho comment
                var commentClientGroup = new ClientGroup()
                {
                    Name = comment.Id.ToString()
                };
                commentClientGroup.UserIds.Add(comment.AuthorId);
                await clientGroupRepository.AddAsync(commentClientGroup);

                #endregion

                //Update again
                return PostAdapter.ToResponse(comment, request.PostId);
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<string> DeleteComment(string commentId)
        {
            var currentComment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            if (currentComment != null)
            {
                currentComment.Status = ItemStatus.Deleted;
                await commentRepository.UpdateAsync(currentComment, currentComment.Id);

                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(currentComment.PostId));
                currentPost.CommentCount--;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                return "Xóa bình luận thành công";
            }
            else throw new Exception("Comment không tồn tại hoặc đã bị xóa");
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
            else throw new Exception("Câu rả lời không tồn tại hoặc đã bị xóa");
        }


        public async Task<IEnumerable<Comment>> GetCommentByPostId(CommentFilterRequest request)
        {
            if (String.IsNullOrEmpty(request.PostId))
                throw new Exception("Không tồn tại bài post");

            var builder = Builders<Comment>.Filter;
            var filter = builder.Eq("post_id", request.PostId) & builder.Eq("status", ItemStatus.Active);

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var comments = await commentRepository.FindListAsync(filter);
            if (comments != null)
            {
                var cmts = comments.AsEnumerable(); 

                if(request.Skip.HasValue && request.Count.HasValue)
                    cmts = cmts.Skip(request.Skip.Value).Take(request.Count.Value);

                if(request.Filter.HasValue && request.ArrangeType.HasValue)
                {
                    switch (request.Filter.Value)
                    {
                        case CommentFilterType.CreatedDate:
                            {
                                if (request.ArrangeType.Value == ArrangeType.Ascending)
                                    cmts = cmts.OrderBy(x => x.CreatedDate);
                                else cmts = cmts.OrderByDescending(x => x.CreatedDate);
                                break;
                            }
                        case CommentFilterType.Upvote:
                            {
                                if (request.ArrangeType.Value == ArrangeType.Ascending)
                                    cmts = cmts.OrderBy(x => x.UpvoteCount);
                                else cmts = cmts.OrderByDescending(x => x.UpvoteCount);
                                break;
                            }
                        default:
                            cmts = cmts.OrderBy(x => x.CreatedDate);
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(request.Keyword))
                    cmts = cmts.Where(x => x.Content.Contains(request.Keyword));

                foreach (var item in cmts)
                {
                    var builderUpvote = Builders<UpVote>.Filter;
                    var finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) & builderUpvote.Eq("upvote_by", currentUser.OId);
                    var upvote = await upVoteRepository.FindAsync(finderUpvote);
                    if (upvote != null)
                        item.IsVoteByCurrent = true;

                    var builderDownvote = Builders<DownVote>.Filter;
                    var finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) & builderDownvote.Eq("downvote_by", currentUser.OId);
                    var downvote = await downVoteRepository.FindAsync(finderDownvote);
                    if (downvote != null)
                        item.IsDownVoteByCurrent = true;
                }
                return cmts;
            }
            else throw new Exception("Post không tồn tại hoặc đã bị xóa");
        }
        public async Task<IEnumerable<ReplyComment>> GetReplyCommentByCommentId(string commentId, int skip, int count)
        {
            var builder = Builders<ReplyComment>.Filter;
            var filter = builder.Eq("parent_id", commentId) & builder.Eq("status", ItemStatus.Active);
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var comments = await replyCommentRepository.FindListAsync(filter);
            if (comments != null)
            {
                var cmts = comments.Skip(skip).Take(count);
                foreach (var item in cmts)
                {
                    var builderUpvote = Builders<UpVote>.Filter;
                    var finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) & builderUpvote.Eq("upvote_by", currentUser.OId);
                    var upvote = await upVoteRepository.FindAsync(finderUpvote);
                    if (upvote != null)
                        item.IsVoteByCurrent = true;

                    var builderDownvote = Builders<DownVote>.Filter;
                    var finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) & builderDownvote.Eq("downvote_by", currentUser.OId);
                    var downvote = await downVoteRepository.FindAsync(finderDownvote);
                    if (downvote != null)
                        item.IsDownVoteByCurrent = true;
                }
                return cmts;
            }
            else throw new Exception("Bình luận không tồn tại hoặc đã bị xóa");
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            ReplyComment replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                await replyCommentRepository.AddAsync(replyComment);
                comment.RepliesCount++;
                await commentRepository.UpdateAsync(comment, comment.Id);

                return PostAdapter.ToResponseReply(replyComment);
            }
            else throw new Exception("Bình luận đã bị xóa");

        }
        public async Task<string> UpvoteComment(string commentId)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var builder = Builders<DownVote>.Filter;

            var downvoteFinder = builder.Eq("object_vote_id", commentId) & builder.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);
            if (existDownvote != null)
                await downVoteRepository.DeleteAsync(existDownvote.Id);

            var upvotebuilder = Builders<UpVote>.Filter;
            var upvoteFinder = upvotebuilder.Eq("object_vote_id", commentId) & upvotebuilder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(upvoteFinder);

            if (existUpvote == null)
            {
                var upvote = new UpVote()
                {
                    UpVoteBy = currentUser.OId,
                    ObjectVoteId = commentId,
                };

                await upVoteRepository.AddAsync(upvote);
                return "Upvote thành công";
            }

            else return "Bạn đã upvote rồi";
        }

        public async Task<string> DownvoteComment(string commentId)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var builder = Builders<UpVote>.Filter;
            var finder = builder.Eq("object_vote_id", commentId) & builder.Eq("upvote_by", currentUser.OId);
            var existUpvote = await upVoteRepository.FindAsync(finder);

            if (existUpvote != null)
                await upVoteRepository.DeleteAsync(existUpvote.Id);


            var builderDownVote = Builders<DownVote>.Filter;

            var downvoteFinder = builderDownVote.Eq("object_vote_id", commentId) & builderDownVote.Eq("downvote_by", currentUser.OId);

            var existDownvote = await downVoteRepository.FindAsync(downvoteFinder);

            if (existDownvote == null)
            {
                var downVote = new DownVote()
                {
                    DownVoteBy = currentUser.OId,
                    ObjectVoteId = commentId,
                };

                await downVoteRepository.AddAsync(downVote);
                return "Downvote thành công";
            }
            else return "Bạn đã downvote rồi";
        }

    }
}

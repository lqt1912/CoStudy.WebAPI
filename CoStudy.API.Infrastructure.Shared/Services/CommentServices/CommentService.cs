using CoStudy.API.Application.FCM;
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
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                Comment comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
                comment.AuthorAvatar = currentUser.AvatarHash;
                comment.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

                currentPost.CommentCount++;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                await commentRepository.AddAsync(comment);

                #region Notification
                FilterDefinition<ClientGroup> filter = Builders<ClientGroup>.Filter.Eq("name", currentPost.OId);
                ClientGroup clientGroup = await clientGroupRepository.FindAsync(filter);

                if (!clientGroup.UserIds.Contains(currentUser.OId))
                {
                    clientGroup.UserIds.Add(currentUser.OId);
                    await clientGroupRepository.UpdateAsync(clientGroup, clientGroup.Id);
                }

                if (currentPost.AuthorId != currentUser.OId) //Cùng tác giả
                {
                    Noftication notify = new Noftication()
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
                    Noftication notify = new Noftication()
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
                ClientGroup commentClientGroup = new ClientGroup()
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
            Comment currentComment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            if (currentComment != null)
            {
                currentComment.Status = ItemStatus.Deleted;
                await commentRepository.UpdateAsync(currentComment, currentComment.Id);

                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(currentComment.PostId));
                currentPost.CommentCount--;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                return "Xóa bình luận thành công";
            }
            else throw new Exception("Comment không tồn tại hoặc đã bị xóa");
        }

        public async Task<string> DeleteReply(string replyId)
        {
            ReplyComment currentReply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(replyId));
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

            FilterDefinitionBuilder<Comment> builder = Builders<Comment>.Filter;
            FilterDefinition<Comment> filter = builder.Eq("post_id", request.PostId) & builder.Eq("status", ItemStatus.Active);

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<Comment> comments = await commentRepository.FindListAsync(filter);
            if (comments != null)
            {
                IEnumerable<Comment> cmts = comments.AsEnumerable();

                if (request.Skip.HasValue && request.Count.HasValue)
                    cmts = cmts.Skip(request.Skip.Value).Take(request.Count.Value);

                if (request.Filter.HasValue && request.ArrangeType.HasValue)
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

                foreach (Comment item in cmts)
                {
                    FilterDefinitionBuilder<UpVote> builderUpvote = Builders<UpVote>.Filter;
                    FilterDefinition<UpVote> finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) & builderUpvote.Eq("upvote_by", currentUser.OId);
                    UpVote upvote = await upVoteRepository.FindAsync(finderUpvote);
                    if (upvote != null)
                        item.IsVoteByCurrent = true;

                    FilterDefinitionBuilder<DownVote> builderDownvote = Builders<DownVote>.Filter;
                    FilterDefinition<DownVote> finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) & builderDownvote.Eq("downvote_by", currentUser.OId);
                    DownVote downvote = await downVoteRepository.FindAsync(finderDownvote);
                    if (downvote != null)
                        item.IsDownVoteByCurrent = true;
                }
                return cmts;
            }
            else throw new Exception("Post không tồn tại hoặc đã bị xóa");
        }
        public async Task<IEnumerable<ReplyComment>> GetReplyCommentByCommentId(string commentId, int skip, int count)
        {
            FilterDefinitionBuilder<ReplyComment> builder = Builders<ReplyComment>.Filter;
            FilterDefinition<ReplyComment> filter = builder.Eq("parent_id", commentId) & builder.Eq("status", ItemStatus.Active);
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<ReplyComment> comments = await replyCommentRepository.FindListAsync(filter);
            if (comments != null)
            {
                IEnumerable<ReplyComment> cmts = comments.Skip(skip).Take(count);
                foreach (ReplyComment item in cmts)
                {
                    FilterDefinitionBuilder<UpVote> builderUpvote = Builders<UpVote>.Filter;
                    FilterDefinition<UpVote> finderUpvote = builderUpvote.Eq("object_vote_id", item.OId) & builderUpvote.Eq("upvote_by", currentUser.OId);
                    UpVote upvote = await upVoteRepository.FindAsync(finderUpvote);
                    if (upvote != null)
                        item.IsVoteByCurrent = true;

                    FilterDefinitionBuilder<DownVote> builderDownvote = Builders<DownVote>.Filter;
                    FilterDefinition<DownVote> finderDownvote = builderDownvote.Eq("object_vote_id", item.OId) & builderDownvote.Eq("downvote_by", currentUser.OId);
                    DownVote downvote = await downVoteRepository.FindAsync(finderDownvote);
                    if (downvote != null)
                        item.IsDownVoteByCurrent = true;
                }
                return cmts;
            }
            else throw new Exception("Bình luận không tồn tại hoặc đã bị xóa");
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            ReplyComment replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
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
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            FilterDefinitionBuilder<DownVote> builder = Builders<DownVote>.Filter;

            FilterDefinition<DownVote> downvoteFinder = builder.Eq("object_vote_id", commentId) & builder.Eq("downvote_by", currentUser.OId);

            DownVote existDownvote = await downVoteRepository.FindAsync(downvoteFinder);
            if (existDownvote != null)
                await downVoteRepository.DeleteAsync(existDownvote.Id);

            FilterDefinitionBuilder<UpVote> upvotebuilder = Builders<UpVote>.Filter;
            FilterDefinition<UpVote> upvoteFinder = upvotebuilder.Eq("object_vote_id", commentId) & upvotebuilder.Eq("upvote_by", currentUser.OId);
            UpVote existUpvote = await upVoteRepository.FindAsync(upvoteFinder);

            if (existUpvote == null)
            {
                UpVote upvote = new UpVote()
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

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            FilterDefinitionBuilder<UpVote> builder = Builders<UpVote>.Filter;
            FilterDefinition<UpVote> finder = builder.Eq("object_vote_id", commentId) & builder.Eq("upvote_by", currentUser.OId);
            UpVote existUpvote = await upVoteRepository.FindAsync(finder);

            if (existUpvote != null)
                await upVoteRepository.DeleteAsync(existUpvote.Id);


            FilterDefinitionBuilder<DownVote> builderDownVote = Builders<DownVote>.Filter;

            FilterDefinition<DownVote> downvoteFinder = builderDownVote.Eq("object_vote_id", commentId) & builderDownVote.Eq("downvote_by", currentUser.OId);

            DownVote existDownvote = await downVoteRepository.FindAsync(downvoteFinder);

            if (existDownvote == null)
            {
                DownVote downVote = new DownVote()
                {
                    DownVoteBy = currentUser.OId,
                    ObjectVoteId = commentId,
                };

                await downVoteRepository.AddAsync(downVote);
                return "Downvote thành công";
            }
            else return "Bạn đã downvote rồi";
        }

        public async Task<Comment> UpdateComment(UpdateCommentRequest request)
        {
            Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (comment == null)
                throw new Exception("Không tìm thấy bình luận");

            comment.Content = request.Content;
            comment.Image = request.Image;
            comment.ModifiedDate = DateTime.Now;
            comment.IsEdited = true;
            await commentRepository.UpdateAsync(comment, comment.Id);
            return comment;
        }

        public async Task<ReplyComment> UpdateReply(UpdateReplyRequest request)
        {
            ReplyComment reply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (reply == null)
                throw new Exception("Không tìm thấy câu trả lời");
            reply.Content = request.Content;
            reply.IsEdited = true;
            reply.ModifiedDate = DateTime.Now;
            await replyCommentRepository.UpdateAsync(reply, reply.Id);
            return reply;
        }
    }
}

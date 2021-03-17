using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
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
    /// <summary>
    /// The comment service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.ICommentService" />
    public class CommentService : ICommentService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;
        /// <summary>
        /// The post repository
        /// </summary>
        private IPostRepository postRepository;
        /// <summary>
        /// The comment repository
        /// </summary>
        private ICommentRepository commentRepository;
        /// <summary>
        /// The reply comment repository
        /// </summary>
        private IReplyCommentRepository replyCommentRepository;
        /// <summary>
        /// Down vote repository
        /// </summary>
        private IDownVoteRepository downVoteRepository;
        /// <summary>
        /// Up vote repository
        /// </summary>
        private IUpVoteRepository upVoteRepository;
        /// <summary>
        /// The client group repository
        /// </summary>
        private IClientGroupRepository clientGroupRepository;
        /// <summary>
        /// The FCM repository
        /// </summary>
        private IFcmRepository fcmRepository;
        /// <summary>
        /// The noftication repository
        /// </summary>
        private INofticationRepository nofticationRepository;

        private IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="postRepository">The post repository.</param>
        /// <param name="commentRepository">The comment repository.</param>
        /// <param name="replyCommentRepository">The reply comment repository.</param>
        /// <param name="downVoteRepository">Down vote repository.</param>
        /// <param name="upVoteRepository">Up vote repository.</param>
        /// <param name="clientGroupRepository">The client group repository.</param>
        /// <param name="fcmRepository">The FCM repository.</param>
        /// <param name="nofticationRepository">The noftication repository.</param>
        public CommentService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IReplyCommentRepository replyCommentRepository,
            IDownVoteRepository downVoteRepository,
            IUpVoteRepository upVoteRepository,
            IClientGroupRepository clientGroupRepository,
            IFcmRepository fcmRepository,
            INofticationRepository nofticationRepository, IMapper mapper)
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
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Post đã bị xóa</exception>
        public async Task<CommentViewModel> AddComment(AddCommentRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var author = await userRepository.GetByIdAsync(ObjectId.Parse(currentPost.AuthorId));
                Comment comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());

                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                await commentRepository.AddAsync(comment);

                try
                {
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
                            Content = $"{currentUser.LastName} đã bình luận bài viết của {author.LastName} ",
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
                }
                catch(Exception)
                {
                    //Do nothing 
                }

                //Update again
                return mapper.Map<CommentViewModel>(comment);
            }
            else throw new Exception("Post đã bị xóa");
        }

        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Comment không tồn tại hoặc đã bị xóa</exception>
        public async Task<string> DeleteComment(string commentId)
        {
            Comment currentComment = await commentRepository.GetByIdAsync(ObjectId.Parse(commentId));
            if (currentComment != null)
            {
                currentComment.Status = ItemStatus.Deleted;
                await commentRepository.UpdateAsync(currentComment, currentComment.Id);

                Post currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(currentComment.PostId));
                await postRepository.UpdateAsync(currentPost, currentPost.Id);
                return "Xóa bình luận thành công";
            }
            else throw new Exception("Comment không tồn tại hoặc đã bị xóa");
        }

        /// <summary>
        /// Deletes the reply.
        /// </summary>
        /// <param name="replyId">The reply identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Câu rả lời không tồn tại hoặc đã bị xóa</exception>
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

        /// <summary>
        /// Gets the comment by post identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Không tồn tại bài post
        /// or
        /// Post không tồn tại hoặc đã bị xóa
        /// </exception>
        public async Task<IEnumerable<CommentViewModel>> GetCommentByPostId(CommentFilterRequest request)
        {
            if (String.IsNullOrEmpty(request.PostId))
                throw new Exception("Không tồn tại bài post");

            FilterDefinitionBuilder<Comment> builder = Builders<Comment>.Filter;
            FilterDefinition<Comment> filter = builder.Eq("post_id", request.PostId) & builder.Eq("status", ItemStatus.Active);

            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<Comment> comments = await commentRepository.FindListAsync(filter);
            if (comments != null)
            {
                IEnumerable<CommentViewModel> cmts = mapper.Map<IEnumerable<CommentViewModel>>(comments.AsEnumerable());

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

                foreach (CommentViewModel item in cmts)
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
        /// <summary>
        /// Gets the reply comment by comment identifier.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Bình luận không tồn tại hoặc đã bị xóa</exception>
        public async Task<IEnumerable<ReplyCommentViewModel>> GetReplyCommentByCommentId(string commentId, int skip, int count)
        {
            FilterDefinitionBuilder<ReplyComment> builder = Builders<ReplyComment>.Filter;
            FilterDefinition<ReplyComment> filter = builder.Eq("parent_id", commentId) & builder.Eq("status", ItemStatus.Active);
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            List<ReplyCommentViewModel> comments = mapper.Map<List<ReplyCommentViewModel>>(await replyCommentRepository.FindListAsync(filter));
            if (comments != null)
            {
                IEnumerable<ReplyCommentViewModel> cmts = comments.Skip(skip).Take(count);
                foreach (ReplyCommentViewModel item in cmts)
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

        /// <summary>
        /// Replies the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Bình luận đã bị xóa</exception>
        public async Task<ReplyCommentViewModel> ReplyComment(ReplyCommentRequest request)
        {
            User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            ReplyComment replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                await replyCommentRepository.AddAsync(replyComment);
                await commentRepository.UpdateAsync(comment, comment.Id);

                return  mapper.Map<ReplyCommentViewModel> (replyComment);
            }
            else throw new Exception("Bình luận đã bị xóa");

        }
        /// <summary>
        /// Upvotes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Downvotes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy bình luận</exception>
        public async Task<CommentViewModel> UpdateComment(UpdateCommentRequest request)
        {
            Comment comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (comment == null)
                throw new Exception("Không tìm thấy bình luận");

            comment.Content = request.Content;
            comment.Image = request.Image;
            comment.ModifiedDate = DateTime.Now;
            comment.IsEdited = true;
            await commentRepository.UpdateAsync(comment, comment.Id);
            return mapper.Map<CommentViewModel>(comment);
        }

        /// <summary>
        /// Updates the reply.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Không tìm thấy câu trả lời</exception>
        public async Task<ReplyCommentViewModel> UpdateReply(UpdateReplyRequest request)
        {
            ReplyComment reply = await replyCommentRepository.GetByIdAsync(ObjectId.Parse(request.Id));
            if (reply == null)
                throw new Exception("Không tìm thấy câu trả lời");
            reply.Content = request.Content;
            reply.IsEdited = true;
            reply.ModifiedDate = DateTime.Now;
            await replyCommentRepository.UpdateAsync(reply, reply.Id);
            return mapper.Map<ReplyCommentViewModel>(reply);
        }
    }
}

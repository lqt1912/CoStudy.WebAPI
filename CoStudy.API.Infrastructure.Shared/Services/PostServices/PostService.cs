using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    public class PostService : IPostService
    {
        IHttpContextAccessor httpContextAccessor;
        IConfiguration configuration;
        IUserRepository userRepository;
        IPostRepository postRepository;
        ICommentRepository commentRepository;
        IReplyCommentRepository replyCommentRepository;
        IFieldRepository fieldRepository;

        public PostService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IReplyCommentRepository replyCommentRepository, IFieldRepository fieldRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
            this.fieldRepository = fieldRepository;
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
                //Update again
                return PostAdapter.ToResponse(comment, request.PostId);
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<AddMediaResponse> AddMedia(AddMediaRequest request)
        {
            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var image = PostAdapter.FromRequest(request, httpContextAccessor);
                currentPost.MediaContents.Add(image);
                currentPost.ModifiedDate = DateTime.Now;
                await postRepository.UpdateAsync(currentPost, currentPost.Id);

                return PostAdapter.ToResponse(image, currentPost.Id.ToString());
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            currentUser.PostCount++;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            var post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();
            post.AuthorAvatar = currentUser.AvatarHash;

            post.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";

            foreach (var fieldId in request.Fields)
            {
                var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
                if (field != null)
                    post.Fields.Add(field);
            }

            await postRepository.AddAsync(post);

            return PostAdapter.ToResponse(post, currentUser.Id.ToString());
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



        public List<Comment> GetCommentByPostId(string postId)
        {
            var comments = commentRepository.GetAll().Where(x => x.PostId == postId && x.Status == ItemStatus.Active).ToList();
            if (comments != null)
                return comments;
            return null;
        }

        public async Task<GetPostByIdResponse> GetPostById(string postId)
        {
            var post = await postRepository.GetByIdAsync(ObjectId.Parse(postId));
            if (post == null || post.Status != ItemStatus.Active)
                throw new Exception("Không tìm thấy bài viết ");
            return PostAdapter.ToResponse(post);
        }

        public GetPostsByUserIdResponse GetPostByUserId(string userId)
        {

            var result = postRepository.GetAll().Where(x => x.AuthorId == userId && x.Status == ItemStatus.Active).ToList();
            if (result != null)
                return new GetPostsByUserIdResponse()
                {
                    Posts = result
                };
            else throw new Exception("Người dùng chưa có bài viết nào");

        }

        public List<Post> GetPostTimeline()
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var listAuthor = currentUser.Followers;
            listAuthor.Add(currentUser.Id.ToString());
            var result = new List<Post>();
            foreach (var post in postRepository.GetAll())
            {
                if (listAuthor.Contains(post.AuthorId) && post.Status == ItemStatus.Active)
                    result.Add(post);
            }
            return result;
        }

        public List<ReplyComment> GetReplyCommentByCommentId(string commentId)
        {
            var comments = replyCommentRepository.GetAll().Where(x => x.ParentId == commentId && x.Status == ItemStatus.Active).ToList();
            if (comments != null)
                return comments;
            return null;
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null && comment.Status == ItemStatus.Active)
            {
                await replyCommentRepository.AddAsync(replyComment);

                return PostAdapter.ToResponseReply(replyComment);
            }
            else throw new Exception("Bình luận đã bị xóa");

        }

        public async Task SyncComment()
        {
            try
            {
                var posts = postRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (var post in posts)
                {
                    var latestComments = commentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.PostId == post.Id.ToString() && x.Status == ItemStatus.Active);
                    if (latestComments.Count() > 3)
                        latestComments = latestComments.Take(3);
                    post.Comments.Clear();
                    post.Comments.AddRange(latestComments);
                    await postRepository.UpdateAsync(post, post.Id);
                }
            }
            catch (Exception)
            {
                //do nothing
                return;
            }
        }

        public async Task SyncReply()
        {
            try
            {
                var comments = commentRepository.GetAll().Where(x => x.Status == ItemStatus.Active).ToList();
                foreach (var comment in comments)
                {
                    var latestReplies = replyCommentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.ParentId == comment.Id.ToString() && x.Status == ItemStatus.Active);
                    if (latestReplies.Count() > 3)
                        latestReplies = latestReplies.Take(3);
                    comment.Replies.Clear();
                    comment.Replies.AddRange(latestReplies);
                    await commentRepository.UpdateAsync(comment, comment.Id);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<string> Upvote(string postId)
        {
            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                //Chưa unlike
                if (!currentUser.PostUpvote.Contains(postId))
                {

                    if (!currentUser.PostDownvote.Contains(postId))
                    {

                        currentPost.Upvote++;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);

                        currentUser.PostUpvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    }
                    else if (currentUser.PostDownvote.Contains(postId))
                    {
                        currentPost.Downvote--;
                        currentPost.Upvote++;
                        await postRepository.UpdateAsync(currentPost, currentPost.Id);

                        currentUser.PostDownvote.Remove(postId);
                        currentUser.PostUpvote.Add(postId);
                        await userRepository.UpdateAsync(currentUser, currentUser.Id);
                    }
                }
                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }
        public async Task<string> Downvote(string postId)
        {

            try
            {
                var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
                var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(postId));

                //chưa like
                if(!currentUser.PostDownvote.Contains(postId))
                { 
                if (!currentUser.PostUpvote.Contains(postId))
                {
                    currentPost.Downvote++;
                    await postRepository.UpdateAsync(currentPost, currentPost.Id);
                    currentUser.PostDownvote.Add(postId);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);

                }
                else if (currentUser.PostUpvote.Contains(postId))
                {
                    currentPost.Downvote++;
                    currentPost.Upvote--;
                    await postRepository.UpdateAsync(currentPost, currentPost.Id);

                    currentUser.PostUpvote.Remove(postId);
                    currentUser.PostDownvote.Add(postId);
                    await userRepository.UpdateAsync(currentUser, currentUser.Id);
                }
                }
                return "Success";
            }
            catch (Exception)
            {
                throw new Exception("Uncompleted activity");
            }
        }
    }
}

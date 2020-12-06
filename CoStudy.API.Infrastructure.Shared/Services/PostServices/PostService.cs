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
using System.Text;
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

        public PostService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IReplyCommentRepository replyCommentRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
            this.replyCommentRepository = replyCommentRepository;
        }

        public async Task<AddCommentResponse> AddComment(AddCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if (currentPost != null)
            {
                var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());

                await commentRepository.AddAsync(comment);
                //Update again
                return PostAdapter.ToResponse(comment, request.PostId);
            }
            else throw new Exception("Post đã bị xóa");
        }

        public async Task<AddMediaResponse> AddMedia(AddMediaRequest request)
        {
            var currentPost =await  postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            if(currentPost!=null)
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

            var currentUser = Feature.CurrentUser(httpContextAccessor,userRepository);
            var post = PostAdapter.FromRequest(request);
            post.AuthorId = currentUser.Id.ToString();
            await postRepository.AddAsync(post);

            return PostAdapter.ToResponse(post, currentUser.Id.ToString());
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var replyComment = PostAdapter.FromRequest(request, currentUser.Id.ToString());
            //Check commenr exist 
            var comment = await commentRepository.GetByIdAsync(ObjectId.Parse(request.ParentCommentId));
            if (comment != null)
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
                var posts = postRepository.GetAll();
                foreach (var post in posts)
                {
                    var latestComments = commentRepository.GetAll().OrderByDescending(x=>x.CreatedDate).Where(x => x.PostId == post.Id.ToString());
                    if (latestComments.Count() > 3)
                        latestComments =  latestComments.Take(3);
                    post.Comments.Clear();
                    post.Comments.AddRange(latestComments);
                    await postRepository.UpdateAsync(post, post.Id);
                }

            } catch(Exception)
            {
                //do nothing
                return;
            }
        }

        public async Task SyncReply()
        {
            try
            {
                var comments = commentRepository.GetAll();
                foreach (var comment in comments)
                {
                    var latestReplies = replyCommentRepository.GetAll().OrderByDescending(x => x.CreatedDate).Where(x => x.ParentId == comment.Id.ToString());
                    if (latestReplies.Count() > 3)
                        latestReplies =  latestReplies.Take(3);
                    comment.Replies.Clear();
                    comment.Replies.AddRange(latestReplies);
                    await commentRepository.UpdateAsync(comment, comment.Id);
                }
            } catch(Exception)
            {
                return;
            }
        }
    }
}

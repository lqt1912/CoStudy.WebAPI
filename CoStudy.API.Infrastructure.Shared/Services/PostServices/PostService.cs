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

        public PostService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        public async Task<AddCommentResponse> AddComment(AddCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));
            var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());

            //Clear comment
            currentPost.Comments.Clear();
            //Update again
            currentPost.Comments.AddRange(commentRepository.GetAll(10));

            await postRepository.UpdateAsync(currentPost, currentPost.Id);

            return PostAdapter.ToResponse(comment, currentPost.Id.ToString());
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
            return null;
        }

        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor,userRepository);
            var post = PostAdapter.FromRequest(request);
            await postRepository.AddAsync(post);

            currentUser.Posts.Clear();
            currentUser.Posts.AddRange(postRepository.GetAll(5));

            currentUser.ModifiedDate = DateTime.Now;
            await userRepository.UpdateAsync(currentUser, currentUser.Id);

            return PostAdapter.ToResponse(post, currentUser.Id.ToString());
        }

        public async Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var comment = PostAdapter.FromRequest(request, currentUser.Id.ToString());

            var currentPost = await postRepository.GetByIdAsync(ObjectId.Parse(request.PostId));

            var rootComment =currentPost.Comments.SingleOrDefault(x=>x.Id == ObjectId.Parse(request.ParentCommentId));
         
            rootComment.Replies.Add(comment);

            await postRepository.UpdateAsync(currentPost, currentPost.Id);

            return PostAdapter.ToResponseReply(comment, rootComment.Id.ToString());
        }
    }
}

using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using System;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class PostAdapter
    {
        public static Post FromRequest(AddPostRequest request)
        {

            Post post = new Post
            {
                Title = request.Title,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = ItemStatus.Active
            };
            if (request.StringContents != null)
            {
                foreach (PostContent content in request.StringContents)
                {
                    PostContent postContent = new PostContent();
                    postContent.ContentType = content.ContentType;
                    postContent.Content = content.Content;
                    post.StringContents.Add(postContent);
                }
            }

            if (request.MediaContents != null)
            {

                foreach (Image content in request.MediaContents)
                {
                    Image postContent = new Image();
                    postContent.ImageHash = content.ImageHash;
                    postContent.Discription = content.Discription;
                    post.MediaContents.Add(postContent);
                }
            }

            return post;
        }

        public static AddPostResponse ToResponse(Post post, string UserId)
        {
            return new AddPostResponse()
            {
                Post = post,
                UserId = UserId
            };
        }

        public static Image FromRequest(AddMediaRequest request, IHttpContextAccessor httpContextAccessor)
        {
            string url = Feature.SaveImage(request.Image, httpContextAccessor, "PostImage");

            return new Image()
            {
                Discription = request.Discription,
                ImageUrl = url,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
        }

        public static AddMediaResponse ToResponse(Image image, string postId)
        {
            return new AddMediaResponse()
            {
                PostId = postId,
                MediaUrl = image.ImageUrl,
                Discription = image.Discription
            };
        }

        public static Comment FromRequest(AddCommentRequest request, string UserId)
        {
            return new Comment()
            {
                AuthorId = UserId,
                PostId = request.PostId,
                Content = request.Content,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Image = request.Image
            };
        }


        public static AddCommentResponse ToResponse(Comment comment, string postId)
        {
            return new AddCommentResponse()
            {
                Comment = comment,
                PostId = postId
            };
        }

        public static ReplyComment FromRequest(ReplyCommentRequest request, string UserId)
        {
            return new ReplyComment()
            {
                AuthorId = UserId,

                ParentId = request.ParentCommentId,
                Content = request.Content,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }


        public static ReplyCommentResponse ToResponseReply(ReplyComment comment)
        {
            return new ReplyCommentResponse()
            {
                Content = comment.Content,
                AuthorId = comment.AuthorId,
                ParentCommentId = comment.ParentId
            };
        }


    }
}

using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using System;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class PostAdapter
    {
        public static Post FromRequest(AddPostRequest request)
        {

            var post = new Post();
            post.Title = request.Title;
            post.Upvote = 0;
            post.Downvote = 0;
            post.CreatedDate = DateTime.Now;
            post.ModifiedDate = DateTime.Now;
            post.Status = ItemStatus.Active;
            foreach (var content in request.StringContents)
            {
                PostContent postContent = new PostContent();
                postContent.ContentType = content.ContentType;
                postContent.Content = content.Content;
                post.StringContents.Add(postContent);
            }

            foreach (var content in request.MediaContents)
            {
                Image postContent = new Image();
                postContent.ImageHash = content.ImageHash;
                postContent.Discription = content.Discription;
                post.MediaContents.Add(postContent);
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
            var url = Feature.SaveImage(request.Image, httpContextAccessor, "PostImage");

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

        public static GetPostByIdResponse ToResponse(Post post)
        {
            return new GetPostByIdResponse()
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                AuthorId = post.AuthorId,
                Upvote = post.Upvote,
                Downvote = post.Downvote,
                CreatedDate = post.CreatedDate,
                ModifiedDate = post.ModifiedDate,
                StringContents = post.StringContents,
                MediaContents = post.MediaContents,
                Comments = post.Comments,
                Fields = post.Fields
            };
        }
    }
}

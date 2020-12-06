using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

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

            foreach (var content in request.StringContents)
            {
                PostContent postContent = new PostContent();
                postContent.ContentType = content.ContentType;
                postContent.Content = content.Content;
                post.StringContents.Add(postContent);
            }

            return post;
        }

        public static AddPostResponse ToResponse(Post post, string UserId)
        {
            return new AddPostResponse()
            {
                Post = post,
                UserId= UserId
            };
        }

        public static Image FromRequest(AddMediaRequest request, IHttpContextAccessor httpContextAccessor)
        {
            var url = Feature.SaveImage(request.Image, httpContextAccessor,"PostImage");

            return  new Image()
            {
                Discription = request.Discription,
                ImageUrl = url,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
        }

        public static AddMediaResponse ToResponse(Image image, string postId )
        {
            return new AddMediaResponse()
            {
                PostId= postId,
                MediaUrl = image.ImageUrl,
                Discription = image.Discription
            };
        }

        public static Comment FromRequest(AddCommentRequest request, string UserId)
        {
            return new Comment()
            {
                AuthorId = UserId,
                Content = request.Content,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
        

        public static AddCommentResponse ToResponse(Comment comment, string postId)
        {
            return new AddCommentResponse()
            {
                Content = comment.Content,
                AuthorId = comment.AuthorId,
                PostId = postId
            };
        }

        public static Comment FromRequest(ReplyCommentRequest request, string UserId)
        {
            return new Comment()
            {
                AuthorId = UserId,
                Content = request.Content,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }


        public static ReplyCommentResponse ToResponseReply(Comment comment, string parentId)
        {
            return new ReplyCommentResponse()
            {
                Content = comment.Content,
                AuthorId = comment.AuthorId,
                ParentCommentId = parentId
            };
        }

    }
}

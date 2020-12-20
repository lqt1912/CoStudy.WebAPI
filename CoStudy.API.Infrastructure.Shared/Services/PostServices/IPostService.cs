using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    public interface IPostService
    {
        Task<AddPostResponse> AddPost(AddPostRequest request);

        Task<AddMediaResponse> AddMedia(AddMediaRequest request);

        Task<AddCommentResponse> AddComment(AddCommentRequest request);

        Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request);

        Task SyncComment();
        Task SyncReply();

        Task<GetPostByIdResponse> GetPostById(string postId);

        GetPostsByUserIdResponse GetPostByUserId(string userId);
        List<Post> GetPostTimeline();
        List<Comment> GetCommentByPostId(string postId);
        List<ReplyComment> GetReplyCommentByCommentId(string commentId);

        Task<string> DeleteComment(string commentId);

        Task<string> DeleteReply(string replyId);

        Task<string> Upvote(string postId);
        Task<string> Downvote(string postId);

    }

}

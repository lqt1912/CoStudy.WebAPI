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


        Task<GetPostByIdResponse> GetPostById(string postId);

        Task<IEnumerable<Post>> GetPostByUserId(string userId, int skip, int count);
        Task<IEnumerable<Post>> GetPostTimelineAsync(int skip, int count);
        List<Comment> GetCommentByPostId(string postId);
        List<ReplyComment> GetReplyCommentByCommentId(string commentId);

        Task<string> DeleteComment(string commentId);

        Task<string> DeleteReply(string replyId);

        Task<string> Upvote(string postId);
        Task<string> Downvote(string postId);

        Task<Post> UpdatePost(UpdatePostRequest request);
        Task<Post> SavePost(string id);
        Task<List<Post>> GetSavedPost(int skip, int count);

       Task< IEnumerable<Post>> Filter(FilterRequest filterRequest);

        Task<string> UpvoteComment(string commentId);
        Task<string> DownvoteComment(string commentId);

        Task SyncComment();
        Task SyncReply();

        Task SyncVote();
        Task SyncReplyVote();
    }

}

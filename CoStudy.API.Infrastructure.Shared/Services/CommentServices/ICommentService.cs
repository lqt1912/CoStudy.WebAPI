using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface ICommentService
    {
        Task<AddCommentResponse> AddComment(AddCommentRequest request);

        Task<ReplyCommentResponse> ReplyComment(ReplyCommentRequest request);

        Task<Comment> UpdateComment(UpdateCommentRequest request);
        Task<ReplyComment> UpdateReply(UpdateReplyRequest request);

        Task<IEnumerable<Comment>> GetCommentByPostId(CommentFilterRequest request);

        Task<IEnumerable<ReplyComment>> GetReplyCommentByCommentId(string commentId, int skip, int count);

        Task<string> DeleteComment(string commentId);

        Task<string> DeleteReply(string replyId);

        Task<string> UpvoteComment(string commentId);
        Task<string> DownvoteComment(string commentId);

    }
}

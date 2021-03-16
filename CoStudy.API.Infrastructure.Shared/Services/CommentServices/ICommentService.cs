using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface ICommentService
    {
        Task<CommentViewModel> AddComment(AddCommentRequest request);

        Task<ReplyCommentViewModel> ReplyComment(ReplyCommentRequest request);

        Task<CommentViewModel> UpdateComment(UpdateCommentRequest request);

        Task<ReplyCommentViewModel> UpdateReply(UpdateReplyRequest request);

        Task<IEnumerable<CommentViewModel>> GetCommentByPostId(CommentFilterRequest request);

        Task<IEnumerable<ReplyCommentViewModel>> GetReplyCommentByCommentId(string commentId, int skip, int count);

        Task<string> DeleteComment(string commentId);

        Task<string> DeleteReply(string replyId);

        Task<string> UpvoteComment(string commentId);
        Task<string> DownvoteComment(string commentId);

    }
}

using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<CommentViewModel> AddComment(AddCommentRequest request);

        /// <summary>
        /// Replies the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ReplyCommentViewModel> ReplyComment(ReplyCommentRequest request);

        /// <summary>
        /// Updates the comment.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<CommentViewModel> UpdateComment(UpdateCommentRequest request);

        /// <summary>
        /// Updates the reply.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<ReplyCommentViewModel> UpdateReply(UpdateReplyRequest request);

        /// <summary>
        /// Gets the comment by post identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<IEnumerable<CommentViewModel>> GetCommentByPostId(CommentFilterRequest request);

        /// <summary>
        /// Gets the reply comment by comment identifier.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<IEnumerable<ReplyCommentViewModel>> GetReplyCommentByCommentId(string commentId, int skip, int count);

        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        Task<string> DeleteComment(string commentId);

        /// <summary>
        /// Deletes the reply.
        /// </summary>
        /// <param name="replyId">The reply identifier.</param>
        /// <returns></returns>
        Task<string> DeleteReply(string replyId);

        /// <summary>
        /// Upvotes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        Task<string> UpvoteComment(string commentId);
        /// <summary>
        /// Downvotes the comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        Task<string> DownvoteComment(string commentId);

        /// <summary>
        /// Upvotes the reply comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        Task<string> UpvoteReplyComment(string commentId);
        /// <summary>
        /// Downvotes the reply comment.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        Task<string> DownvoteReplyComment(string commentId);
    }
}

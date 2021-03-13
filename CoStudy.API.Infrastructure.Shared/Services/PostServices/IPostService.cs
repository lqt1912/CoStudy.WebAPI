using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<PostViewModel> GetPostById1(string id);

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<PostViewModel> AddPost(AddPostRequest request);

        /// <summary>
        /// Adds the media.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<AddMediaResponse> AddMedia(AddMediaRequest request);

        /// <summary>
        /// Gets the post by identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        Task<GetPostByIdResponse> GetPostById(string postId);

        /// <summary>
        /// Gets the post by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetPostByUserId(string userId, int skip, int count);
        /// <summary>
        /// Gets the post timeline asynchronous.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetPostTimelineAsync(int skip, int count);
        /// <summary>
        /// Upvotes the specified post identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        Task<string> Upvote(string postId);
        /// <summary>
        /// Downvotes the specified post identifier.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        Task<string> Downvote(string postId);

        /// <summary>
        /// Updates the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<Post> UpdatePost(UpdatePostRequest request);
        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Post> SavePost(string id);
        /// <summary>
        /// Gets the saved post.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<List<Post>> GetSavedPost(int skip, int count);

        /// <summary>
        /// Filters the specified filter request.
        /// </summary>
        /// <param name="filterRequest">The filter request.</param>
        /// <returns></returns>
        Task<IEnumerable<Post>> Filter(FilterRequest filterRequest);
        /// <summary>
        /// Synchronizes the comment.
        /// </summary>
        /// <returns></returns>
        Task SyncComment();
        /// <summary>
        /// Synchronizes the reply.
        /// </summary>
        /// <returns></returns>
        Task SyncReply();
        /// <summary>
        /// Synchronizes the vote.
        /// </summary>
        /// <returns></returns>
        Task SyncVote();
        /// <summary>
        /// Synchronizes the reply vote.
        /// </summary>
        /// <returns></returns>
        Task SyncReplyVote();
    }

}

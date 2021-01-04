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

        Task<GetPostByIdResponse> GetPostById(string postId);

        Task<IEnumerable<Post>> GetPostByUserId(string userId, int skip, int count);
        Task<IEnumerable<Post>> GetPostTimelineAsync(int skip, int count);
        Task<string> Upvote(string postId);
        Task<string> Downvote(string postId);

        Task<Post> UpdatePost(UpdatePostRequest request);
        Task<Post> SavePost(string id);
        Task<List<Post>> GetSavedPost(int skip, int count);

        Task<IEnumerable<Post>> Filter(FilterRequest filterRequest);
        Task SyncComment();
        Task SyncReply();
        Task SyncVote();
        Task SyncReplyVote();
    }

}

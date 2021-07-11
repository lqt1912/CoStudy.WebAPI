using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using CoStudy.API.Domain.Entities.Application;

namespace CoStudy.API.Infrastructure.Shared.Services.PostServices
{
    public interface IPostService
    {
        Task<PostViewModel> GetPostById1(string id);
        Task<PostViewModel> AddPost(AddPostRequest request);
        Task<IEnumerable<PostViewModel>> GetPostByUserId(GetPostByUserRequest request);
        Task<IEnumerable<PostViewModel>> GetPostTimelineAsync(BaseGetAllRequest request);
        Task<string> Upvote(string postId);
        Task<string> Downvote(string postId);
        Task<PostViewModel> UpdatePost(UpdatePostRequest request);
        Task<SavePostResponse> SavePost(string id);
        Task<List<PostViewModel>> GetSavedPost(BaseGetAllRequest request);
        Task<PostFilterResponse> Filter(FilterRequest filterRequest);
        Task<IEnumerable<PostViewModel>> GetNewsFeed(NewsFeedRequest request);
        Task<IEnumerable<MessageViewModel>> SharePost(SharePostRequest request);
        Task<PostViewModel> ModifiedPostStatus(ModifedPostStatusRequest request);
        bool IsViolencePost(string postId);
        Task<List<User>> GetUsersMatchPostField(Post post);
        Task<List<UserViewModel>> GetUserByPostField(string postId);
        IEnumerable<SearchHistoryViewModel> GetHistory(BaseGetAllRequest request);
    }
}

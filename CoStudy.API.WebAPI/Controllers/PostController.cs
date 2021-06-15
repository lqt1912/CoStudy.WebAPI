using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {

        IPostService postService;

        ILevelService levelService;
        public PostController(IPostService postService, ILevelService levelService)
        {
            this.postService = postService;
            this.levelService = levelService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest request)
        {
            //var fakeString = "Lorem Ipsum is simply dummy text of the printing and typesetting" +
            //    " industry. Lorem Ipsum has been the industry's standard dummy text ever since" +
            //    " the 1500s, when an unknown printer took a galley of type and scrambled it to " +
            //    "make a type specimen book. It has survived not only five centuries, but also the " +
            //    "leap into electronic typesetting, remaining essentially unchanged. It was " +
            //    "popularised in the 1960s with the release of Letraset sheets containing " +
            //    "Lorem Ipsum passages, and more recently with desktop publishing software " +
            //    "like Aldus PageMaker including versions of Lorem Ipsum.";

            //var strLength = fakeString.Length - 1;
            //Random rand = new Random();
            //Tuple<int, int> title = new Tuple<int, int>(rand.Next(0, 10), rand.Next(20, 50));
            //Tuple<int, int> content = new Tuple<int, int>(rand.Next(0, strLength / 2 - 1), rand.Next(strLength / 2 + 1, strLength-2));



            //var fakeRequest = new AddPostRequest()
            //{
            //    Title = fakeString.Substring(title.Item1, title.Item2),
            //    StringContents = new List<PostContent>()
            //     {
            //         new PostContent()
            //         {
            //             Content  = fakeString.Substring(content.Item1, content.Item2),
            //              ContentType =0

            //             }
            //     },
            //};

            PostViewModel data = await postService.AddPost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            PostViewModel data = await postService.GetPostById1(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> GetByUserId(GetPostByUserRequest request)
        {
            var data = await postService.GetPostByUserId(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("timeline")]
        public async Task<IActionResult> GetPostTimeline(BaseGetAllRequest request)
        {
            var data = await postService.GetPostTimelineAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("news-feed")]
        public async Task<IActionResult> GetNewsFeed(NewsFeedRequest request)
        {
            var data = await postService.GetNewsFeed(request);
            return Ok(new ApiOkResponse(data));
        }


        [HttpPost]
        [Route("upvote/{id}")]
        public async Task<IActionResult> Upvote(string id)
        {
            string data = await postService.Upvote(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("downvote/{id}")]
        public async Task<IActionResult> Downvote(string id)
        {
            string data = await postService.Downvote(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(UpdatePostRequest request)
        {
            var data = await postService.UpdatePost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("save/{id}")]
        public async Task<IActionResult> SavePost(string id)
        {
            var data = await postService.SavePost(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("save")]
        public async Task<IActionResult> GetSavedPost([FromQuery] BaseGetAllRequest request)
        {
            var data = await postService.GetSavedPost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> FilterPost(FilterRequest request)
        {
            var data = await postService.Filter(request);
            return Ok(new ApiOkResponse(data));
        }


        [HttpPost]
        [Route("update-field")]
        public async Task<IActionResult> UpdatePostField(UpdatePostLevelRequest request)
        {
            var data = await levelService.UpdatePostField(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("share-post")]
        public async Task<IActionResult> SharePost(SharePostRequest request)
        {
            var data = await postService.SharePost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("modified-post-status")]
        [Authorize]
        public async Task<IActionResult> ModifiedPostStatus(ModifedPostStatusRequest request)
        {
            var data = await postService.ModifiedPostStatus(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

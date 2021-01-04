using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {

        IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddPost(AddPostRequest request)
        {
            var data = await postService.AddPost(request);
            return Ok(new ApiOkResponse(data));
        }



        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await postService.GetPostById(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("get/user/{userId}/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetByUserId(string userId, int skip, int count)
        {
            var data = await postService.GetPostByUserId(userId, skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("timeline/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetPostTimeline(int skip, int count)
        {
            var data = await postService.GetPostTimelineAsync(skip, count);
            return Ok(new ApiOkResponse(data));
        }



        [HttpPost]
        [Route("post/upvote/{id}")]
        public async Task<IActionResult> Upvote(string id)
        {
            var data = await postService.Upvote(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("post/downvote/{id}")]
        public async Task<IActionResult> Downvote(string id)
        {
            var data = await postService.Downvote(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("post/update")]
        public async Task<IActionResult> Update(UpdatePostRequest request)
        {
            var data = await postService.UpdatePost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("post/save/{id}")]
        public async Task<IActionResult> SavePost(string id)
        {
            var data = await postService.SavePost(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("post/save")]
        public async Task<IActionResult> GetSavedPost(int skip, int count)
        {
            var data = await postService.GetSavedPost(skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("post/filter")]
        public async Task<IActionResult> FilterPost(FilterRequest request)
        {
            var data = await postService.Filter(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

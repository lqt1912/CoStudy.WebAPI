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

        //[HttpPost]
        //[Route("media/add")]
        //public async Task<IActionResult> AddMedia([FromForm] AddMediaRequest request)
        //{
        //    var data = await postService.AddMedia(request);
        //    return Ok(new ApiOkResponse(data));
        //}

        [HttpPost]
        [Route("comment/add")]
        public async Task<IActionResult> AddComment(AddCommentRequest request)
        {
            var data = await postService.AddComment(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("comment/reply")]
        public async Task<IActionResult> Addreply(ReplyCommentRequest request)
        {
            var data = await postService.ReplyComment(request);
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
        [Route("get/user/{userId}")]
        public IActionResult GetByUserId(string userId)
        {
            var data = postService.GetPostByUserId(userId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("timeline")]
        public IActionResult GetPostTimeline()
        {
            var data = postService.GetPostTimeline();
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("comments/{postId}")]
        public IActionResult GetCommentByPostId(string postId)
        {
            var data = postService.GetCommentByPostId(postId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("replies/{commentId}")]
        public IActionResult GetReplyByCommentId(string commentId)
        {
            var data = postService.GetReplyCommentByCommentId(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("comment/{id}")]
        public async Task<IActionResult> DeleteCommentById(string id)
        {
            var data = await postService.DeleteComment(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("reply/{id}")]
        public async Task<IActionResult> DeleteReply(string id)
        {
            var data = await postService.DeleteReply(id);
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
    }
}

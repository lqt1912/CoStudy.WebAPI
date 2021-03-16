using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddComment(AddCommentRequest request)
        {
          var data = await commentService.AddComment(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("reply")]
        public async Task<IActionResult> Addreply(ReplyCommentRequest request)
        {
           var data = await commentService.ReplyComment(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("get/post")]
        public async Task<IActionResult> GetCommentByPostId([FromQuery] CommentFilterRequest request)
        {
           var  data = await commentService.GetCommentByPostId(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("get/replies/{commentId}")]
        public async Task<IActionResult> GetReplyByCommentId(string commentId, int skip, int count)
        {
           var data = await commentService.GetReplyCommentByCommentId(commentId, skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("comment/{id}")]
        public async Task<IActionResult> DeleteCommentById(string id)
        {
            string data = await commentService.DeleteComment(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("reply/{id}")]
        public async Task<IActionResult> DeleteReply(string id)
        {
            string data = await commentService.DeleteReply(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("upvote/{commentId}")]
        public async Task<IActionResult> UpvoteComment(string commentId)
        {
            string data = await commentService.UpvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("downvote/{commentId}")]
        public async Task<IActionResult> DownvoteComment(string commentId)
        {
            string data = await commentService.DownvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("comment/update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentRequest request)
        {
            var  data = await commentService.UpdateComment(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("reply/update")]
        public async Task<IActionResult> UpdateReply(UpdateReplyRequest request)
        {
            var data = await commentService.UpdateReply(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

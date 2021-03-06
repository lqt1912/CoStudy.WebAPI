﻿using CoStudy.API.Infrastructure.Shared.Models.Request;
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
            var data = await commentService.GetCommentByPostId(request);
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
        [Route("upvote-comment/{commentId}")]
        public async Task<IActionResult> UpvoteComment(string commentId)
        {
            string data = await commentService.UpvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("downvote-comment/{commentId}")]
        public async Task<IActionResult> DownvoteComment(string commentId)
        {
            string data = await commentService.DownvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("upvote-reply/{replyId}")]
        public async Task<IActionResult> UpvoteReplyComment(string replyId)
        {
            string data = await commentService.UpvoteReplyComment(replyId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("downvote-reply/{replyId}")]
        public async Task<IActionResult> DownvoteReplyComment(string replyId)
        {
            string data = await commentService.DownvoteReplyComment(replyId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("comment/update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentRequest request)
        {
            var data = await commentService.UpdateComment(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut]
        [Route("reply/update")]
        public async Task<IActionResult> UpdateReply(UpdateReplyRequest request)
        {
            var data = await commentService.UpdateReply(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("commment/{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            var data = await commentService.GetCommentById(id);
            if (data != null)
                return Ok(new ApiOkResponse(data));
            return Ok(new ApiNotFoundResponse("Bình luận không tồn tại hoặc đã bị xóa. "));
        }

        [HttpGet]
        [Route("reply-comment/{id}")]
        public async Task<IActionResult> GetReplyCommentById(string id)
        {
            var data = await commentService.GetReplyCommentById(id);
            if(data !=null)
                 return Ok(new ApiOkResponse(data));
            return Ok(new ApiNotFoundResponse("Phản hồi không tồn tại hoặc đã bị xóa. "));
        }

        [HttpPut("modified-comment-status")]
        public async Task<IActionResult> ModifiedCommentStatus(ModifiedCommentStatusRequest request)
        {
            var data = await commentService.ModifiedCommentStatus(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut("modified-reply-status")]
        public async Task<IActionResult> ModifiedReplyCommentStatus(ModifiedCommentStatusRequest request)
        {
            var data = await commentService.ModifiedReplyCommentStatus(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}
﻿using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("get/{postId}/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetCommentByPostId(string postId, int skip, int count)
        {
            var data = await commentService.GetCommentByPostId(postId, skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("get/replies/{commentId}")]
        public async  Task<IActionResult> GetReplyByCommentId(string commentId, int skip, int count)
        {
            var data = await commentService.GetReplyCommentByCommentId(commentId, skip, count);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("comment/{id}")]
        public async Task<IActionResult> DeleteCommentById(string id)
        {
            var data = await commentService.DeleteComment(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpDelete]
        [Route("reply/{id}")]
        public async Task<IActionResult> DeleteReply(string id)
        {
            var data = await commentService.DeleteReply(id);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("upvote/{commentId}")]
        public async Task<IActionResult> UpvoteComment(string commentId)
        {
            var data = await commentService.UpvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("downvote/{commentId}")]
        public async Task<IActionResult> DownvoteComment(string commentId)
        {
            var data = await commentService.DownvoteComment(commentId);
            return Ok(new ApiOkResponse(data));
        }
    }
}
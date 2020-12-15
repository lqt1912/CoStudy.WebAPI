using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public  async Task<IActionResult> AddPost(AddPostRequest request)
        {
            var data =await  postService.AddPost(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("media/add")]
        public async Task<IActionResult> AddMedia([FromForm]AddMediaRequest request)
        {
            var data = await postService.AddMedia(request);
            return Ok(new ApiOkResponse(data));
        }

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
        public IActionResult GetByUserId(string userId )
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
    }
}

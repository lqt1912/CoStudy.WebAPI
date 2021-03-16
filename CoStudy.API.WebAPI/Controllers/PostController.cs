using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// The Post Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {

        /// <summary>
        /// The post service
        /// </summary>
        IPostService postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostController" /> class.
        /// </summary>
        /// <param name="postService">The post service.</param>
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest request)
        {
            PostViewModel data = await postService.AddPost(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            PostViewModel data = await postService.GetPostById1(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the by user identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> GetByUserId(GetPostByUserRequest request)
        {
           var data = await postService.GetPostByUserId(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the post timeline.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("timeline")]
        public async Task<IActionResult> GetPostTimeline(BaseGetAllRequest request)
        {
          var data = await postService.GetPostTimelineAsync(request);
            return Ok(new ApiOkResponse(data));
        }



        /// <summary>
        /// Upvotes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("upvote/{id}")]
        public async Task<IActionResult> Upvote(string id)
        {
            string data = await postService.Upvote(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Downvotes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("downvote/{id}")]
        public async Task<IActionResult> Downvote(string id)
        {
            string data = await postService.Downvote(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Updates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(UpdatePostRequest request)
        {
           var data = await postService.UpdatePost(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("save/{id}")]
        public async Task<IActionResult> SavePost(string id)
        {
            var data = await postService.SavePost(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the saved post.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("save")]
        public async Task<IActionResult> GetSavedPost([FromQuery]BaseGetAllRequest request)
        {
           var data = await postService.GetSavedPost(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Filters the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> FilterPost(FilterRequest request)
        {
            var data = await postService.Filter(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

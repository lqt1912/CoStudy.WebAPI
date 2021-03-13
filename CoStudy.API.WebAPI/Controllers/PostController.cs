using CoStudy.API.Domain.Entities.Application;
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
        /// Initializes a new instance of the <see cref="PostController"/> class.
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
        [Route("add")]
        public async Task<IActionResult> AddPost(AddPostRequest request)
        {
            AddPostResponse data = await postService.AddPost(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            PostViewModel data = await postService.GetPostById1(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/user/{userId}/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetByUserId(string userId, int skip, int count)
        {
            IEnumerable<Domain.Entities.Application.Post> data = await postService.GetPostByUserId(userId, skip, count);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the post timeline.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("timeline/skip/{skip}/count/{count}")]
        public async Task<IActionResult> GetPostTimeline(int skip, int count)
        {
            System.Collections.Generic.IEnumerable<Domain.Entities.Application.Post> data = await postService.GetPostTimelineAsync(skip, count);
            return Ok(new ApiOkResponse(data));
        }



        /// <summary>
        /// Upvotes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("post/upvote/{id}")]
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
        [Route("post/downvote/{id}")]
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
        [Route("post/update")]
        public async Task<IActionResult> Update(UpdatePostRequest request)
        {
           Post data = await postService.UpdatePost(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("post/save/{id}")]
        public async Task<IActionResult> SavePost(string id)
        {
            Domain.Entities.Application.Post data = await postService.SavePost(id);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the saved post.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("post/save")]
        public async Task<IActionResult> GetSavedPost(int skip, int count)
        {
            System.Collections.Generic.List<Domain.Entities.Application.Post> data = await postService.GetSavedPost(skip, count);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Filters the post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("post/filter")]
        public async Task<IActionResult> FilterPost(FilterRequest request)
        {
            System.Collections.Generic.IEnumerable<Domain.Entities.Application.Post> data = await postService.Filter(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

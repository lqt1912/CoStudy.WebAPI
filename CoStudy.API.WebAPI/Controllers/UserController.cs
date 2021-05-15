using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// The User Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService userService;
        /// <summary>
        /// The account service
        /// </summary>
        IAccountService _accountService;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="accountService">The account service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserController(IUserService userService, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            _accountService = accountService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> AddUser(AddUserRequest user)
        {
            var registerRequest = new RegisterRequest()
            {
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword,
                AcceptTerms = user.AcceptTerms
            };

            await _accountService.Register(registerRequest, Feature.GetHostUrl(httpContextAccessor));
            var data = await userService.AddUserAsync(user);

            var response = new
            {
                Data = data,
                Message = "Registration successful, please check your email for verification instructions"
            };

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var data = await userService.UpdateUserAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Uploads the avatar.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> UploadAvatar(AddAvatarRequest request)
        {
            var data = await userService.AddAvatarAsync(request);

            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("avatar/update")]
        public async Task<IActionResult> UpdateAvatar(AddAvatarRequest request)
        {
            var data = await userService.UpdateAvatarAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the followings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("following")]
        public async Task<IActionResult> AddFollowings([FromBody] AddFollowerRequest request)
        {
            var data = await userService.AddFollowingsAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the additional infos.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("additionalinfos")]
        public async Task<IActionResult> AddAdditionalInfos(List<AdditionalInfomation> request)
        {
            var data = await userService.AddOrUpdateInfo(request);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await userService.GetUserById(id);
            return Ok(new ApiOkResponse(data));
        }

        //  [Authorize]
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrentUser()
        {
            var data = userService.GetCurrentUser();
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Unfollows the specified following identifier.
        /// </summary>
        /// <param name="followingId">The following identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("following/remove")]
        public async Task<IActionResult> Unfollow(string followingId)
        {
            var data = await userService.RemoveFollowing(followingId);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the fields.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("field/add/database")]
        public async Task<IActionResult> AddFields(string field)
        {
            var data = await userService.AddField(field);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the follower.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-follower")]
        public async Task<IActionResult> GetFollower(FollowFilterRequest request)
        {
            var data = await userService.GetFollower(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the following.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-following")]
        public async Task<IActionResult> GetFollowing(FollowFilterRequest request)
        {
            var data = await userService.GetFollowing(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets all field.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("field/all")]
        public IActionResult getAllField()
        {
            var data = userService.GetAll();
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Users the filter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/filter")]
        public async Task<IActionResult> UserFilter(FilterUserRequest request)
        {
            var data = await userService.FilterUser(request);
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user/revoke-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            var data = await _accountService.GetCurrentRefreshToken();
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [Route("cache/get")]
        [HttpGet]
        public IActionResult GetCache(string email)
        {
            var data = CacheHelper.GetValue($"CurrentUser-{email}") as User;
            return Ok(new ApiOkResponse(data));
        }


        /// <summary>
        /// Gets the nearby user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet("near-by")]
        public IActionResult GetNearbyUser([FromQuery] BaseGetAllRequest request)
        {
            var data = userService.GetNearbyUser(request);
            return Ok(new ApiOkResponse(data));
        }

        /// <summary>
        /// Adds the or update call identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("call-id")]
        public async Task<IActionResult> AddOrUpdateCallId(AddOrUpdateCallIdRequest request)
        {
            var data = await userService.AddOrUpdateCallId(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}

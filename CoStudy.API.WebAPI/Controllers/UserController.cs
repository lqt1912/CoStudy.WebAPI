using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        IAccountService _accountService;
        IHttpContextAccessor httpContextAccessor;
        public UserController(IUserService userService, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            _accountService = accountService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> AddUser(AddUserRequest user)
        {
            RegisterRequest registerRequest = new RegisterRequest()
            {
                Email = user.Email,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword,
                AcceptTerms = user.AcceptTerms
            };

            await _accountService.Register(registerRequest, Feature.GetHostUrl(httpContextAccessor));
            Infrastructure.Shared.Models.Response.UserResponse.AddUserResponse data = await userService.AddUserAsync(user);

            var response = new
            {
                Data = data,
                Message = "Registration successful, please check your email for verification instructions"
            };

            return Ok(new ApiOkResponse(response));
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            User data = await userService.UpdateUserAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> UploadAvatar(AddAvatarRequest request)
        {
            Infrastructure.Shared.Models.Response.UserResponse.AddAvatarResponse data = await userService.AddAvatarAsync(request);

            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("avatar/update")]
        public async Task<IActionResult> UpdateAvatar(AddAvatarRequest request)
        {
            User data = await userService.UpdateAvatarAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("following")]
        public async Task<IActionResult> AddFollowings([FromBody] AddFollowerRequest request)
        {
            string data = await userService.AddFollowingsAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("additionalinfos")]
        public async Task<IActionResult> AddAdditionalInfos(List<IDictionary<string, string>> request)
        {
            User data = await userService.AddInfo(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("field")]
        public async Task<IActionResult> AddField(AddFieldRequest request)
        {
            User data = await userService.AddFieldAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPut]
        [Route("field")]
        public async Task<IActionResult> UpdateField(AddFieldRequest request)
        {
            User data = await userService.UpdateFieldAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            User data = await userService.GetUserById(id);
            return Ok(new ApiOkResponse(data));
        }

        //  [Authorize]
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrentUser()
        {
            User data = userService.GetCurrentUser();
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("following/remove")]
        public async Task<IActionResult> Unfollow(string followingId)
        {
            string data = await userService.RemoveFollowing(followingId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("field/add/database")]
        public async Task<IActionResult> AddFields(string field)
        {
            Field data = await userService.AddField(field);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("follower")]
        public async Task<IActionResult> GetFollower([FromQuery] FollowFilterRequest request)
        {
            IEnumerable<Follow> data = await userService.GetFollower(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("following")]
        public async Task<IActionResult> GetFollowing([FromQuery] FollowFilterRequest request)
        {
            IEnumerable<Follow> data = await userService.GetFollowing(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("field/all")]
        public IActionResult getAllField()
        {
            List<Field> data = userService.GetAll();
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("user/filter")]
        public async Task<IActionResult> UserFilter(FilterUserRequest request)
        {
            IEnumerable<User> data = await userService.FilterUser(request);
            return Ok(new ApiOkResponse(data));
        }


        [HttpGet]
        [Route("user/revoke-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            string data = await _accountService.GetCurrentRefreshToken();
            return Ok(new ApiOkResponse(data));
        }

        [Route("cache/get")]
        [HttpGet]
        public IActionResult GetCache(string email)
        {
            User data = CacheHelper.GetValue($"CurrentUser-{email}") as User;
            return Ok(new ApiOkResponse(data));
        }
    }
}

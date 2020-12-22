using CoStudy.API.Application.Features;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var data = await userService.UpdateUserAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> UploadAvatar(AddAvatarRequest request)
        {
            var data = await userService.AddAvatarAsync(request);

            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("avatar/update")]
        public async Task<IActionResult> UpdateAvatar(AddAvatarRequest request)
        {
            var data = await userService.UpdateAvatarAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("following")]
        public async Task<IActionResult> AddFollowings([FromBody] AddFollowerRequest request)
        {
            var data = await userService.AddFollowingsAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("additionalinfos")]
        public async Task<IActionResult> AddAdditionalInfos(AddAdditionalInfoRequest request)
        {
            var data = await userService.AddAdditonalInfoAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("field")]
        public async Task<IActionResult> AddField(AddFieldRequest request)
        {
            var data = await userService.AddFieldAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await userService.GetUserById(id);
            return Ok(new ApiOkResponse(data));
        }

        //  [Authorize]
        [HttpGet]
        [Route("current")]
        public IActionResult GetCurrentUser()
        {
            var data = userService.GetCurrentUser();
            return Ok(new ApiOkResponse(data));
        }

        [Authorize]
        [HttpPost]
        [Route("following/remove")]
        public async Task<IActionResult> Unfollow(string followingId)
        {
            var data = await userService.RemoveFollowing(followingId);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("field/add/database")]
        public async Task<IActionResult> AddFields(string field)
        {
            var data = await userService.AddField(field);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("field/all")]
        public IActionResult getAllField()
        {
            var data = userService.GetAll();
            return Ok(new ApiOkResponse(data));
        }
    }
}

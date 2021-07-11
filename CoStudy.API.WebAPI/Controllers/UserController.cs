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
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;

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
                AcceptTerms = user.AcceptTerms,
                IsExternalRegister = user.IsExternalRegister
            };
            var data = await userService.AddUserAsync(user);

            await _accountService.Register(registerRequest, Feature.GetHostUrl(httpContextAccessor));


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
        [HttpPost("update-address")]
        public async Task<IActionResult> UpdateAddress(Address request)
        {
            var data = await userService.UpdateUserAddress(request);
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
        public async Task<IActionResult> AddAdditionalInfos(List<AdditionalInfomation> request)
        {
            var data = await userService.AddOrUpdateInfo(request);
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

        [Authorize]
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

        [HttpPost]
        [Route("get-follower")]
        public async Task<IActionResult> GetFollower(FollowFilterRequest request)
        {
            var data = await userService.GetFollower(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("get-following")]
        public async Task<IActionResult> GetFollowing(FollowFilterRequest request)
        {
            var data = await userService.GetFollowing(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("field/all")]
        public IActionResult getAllField()
        {
            var data = userService.GetAll();
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("user/filter")]
        public async Task<IActionResult> UserFilter(FilterUserRequest request)
        {
            var data = await userService.FilterUser(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("user/revoke-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            var data = await _accountService.GetCurrentRefreshToken();
            return Ok(new ApiOkResponse(data));
        }

        [Route("cache/get")]
        [HttpGet]
        public IActionResult GetCache(string email)
        {
            var data = CacheHelper.GetValue($"CurrentUser-{email}") as User;
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet("near-by")]
        public IActionResult GetNearbyUser([FromQuery] BaseGetAllRequest request)
        {
            var data = userService.GetNearbyUser(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("call-id")]
        public async Task<IActionResult> AddOrUpdateCallId(AddOrUpdateCallIdRequest request)
        {
            var data = await userService.AddOrUpdateCallId(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpGet]
        [Route("is-exist")]
        public async Task<IActionResult> CheckUserExist([FromQuery] string email)
        {
            var data = await userService.IsEmailExist(email);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPut("modified-user")]
        [Authorize(Role.Admin)]
        public async Task<IActionResult> Modified([FromBody] ModifiedRequest request)
        {
            var data = await userService.ModifiedUser(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("remove-notification/{objectId}")]
        public async Task<IActionResult> TurnOffNotification(string objectId)
        {
            await userService.TurnOffNotification(objectId);
            return Ok(new ApiOkResponse("Thành công. "));
        }

        [HttpPost("add-notification/{objectId}")]
        [Authorize]
        public async Task<IActionResult> TurnOnNotification(string objectId)
        {
            await userService.TurnOnNotification(objectId);
            return Ok(new ApiOkResponse("Thành công. "));
        }

        [HttpGet]
        [Route("history")]
        public IActionResult GetHistory([FromQuery] BaseGetAllRequest request)
        {
            var data = userService.GetHistory(request);
            return Ok(new ApiOkResponse(data));
        }
    }
}
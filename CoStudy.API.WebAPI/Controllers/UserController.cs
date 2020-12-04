using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Application.Features;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                Title = user.Title,
                FirstName = user.FisrtName,
                LastName = user.LastName,
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

        [HttpPost]
        [Route("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm]AddAvatarRequest request)
        {
            var data = await userService.AddAvatarAsync(request);

            return Ok(new ApiOkResponse(data));    
        }

        [HttpPost]
        [Route("followers")]
        public async Task<IActionResult> AddFollowers([FromBody] AddFollowerRequest request)
        {
            var data = await userService.AddFollowersAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("followings")]
        public async Task<IActionResult> AddFollowings([FromBody] AddFollowerRequest request)
        {
            var data = await userService.AddFollowingsAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("additionalinfos")]
        public async Task<IActionResult> AddAdditionalInfos(AddAdditionalInfoRequest request)
        {
            var data = await userService.AddAdditonalInfoAsync(request);
            return Ok(new ApiOkResponse(data));
        }

        [HttpPost]
        [Route("field")]
        public async Task<IActionResult> AddField([FromForm]AddFieldRequest request)
        {
            var data = await userService.AddFieldAsync(request);

            return Ok(new ApiOkResponse(data));
        }
    }
}

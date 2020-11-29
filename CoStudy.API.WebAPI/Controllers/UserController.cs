using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Application;
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

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> AddUser(AddUserRequest user)
        {
            var data = await userService.AddUserAsync(user);

            return Ok(new ApiOkResponse(data));
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

    }
}

using AutoMapper;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Identity.Services.GoogleAuthService;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        private readonly IGoogleAuthServices googleAuthServices;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountsController(
            IAccountService accountService,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, IGoogleAuthServices googleAuthServices)
        {
            _accountService = accountService;
            _mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.googleAuthServices = googleAuthServices;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = await _accountService.Authenticate(model, ipAddress());
            return Ok(new ApiOkResponse(response));
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<object>> GoogleLogin(GoogleAuthenticationRequest request)
        {
            var data = await googleAuthServices.ExternalLogin(request, ipAddress());

            return Ok(new ApiOkResponse(data));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken([FromQuery] string refreshToken)
        {
            var response =await _accountService.RefreshToken(refreshToken, ipAddress());
            return Ok(new ApiOkResponse(response));
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)  
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new {message = "Token is required"});
            }

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            {
                return Unauthorized(new {message = "Unauthorized"});
            }

            _accountService.RevokeToken(token, ipAddress());
            return Ok(new ApiOkResponse("Token revoked"));
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterRequest model)
        //{
        //    await _accountService.Register(model, GetHostUrl());
        //    return Ok(new ApiOkResponse( "Registration successful, please check your email for verification instructions" ));
        //}

        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(string token)
        {
            _accountService.VerifyEmail(token);
            return Ok(new ApiOkResponse("Verification successful, you can now login"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, GetHostUrl());

            return Ok(new ApiOkResponse("Please check your email for password reset instructions"));
        }

        [HttpPost("validate-reset-token")]
        public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
        {
            _accountService.ValidateResetToken(model);
            return Ok(new ApiOkResponse("Token is valid"));
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _accountService.ResetPassword(model);
            return Ok(new ApiOkResponse("Password reset successful, you can now login"));
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<AccountResponse>> GetAll()
        {
            var accounts = _accountService.GetAll();
            return Ok(new ApiOkResponse(accounts));
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<AccountResponse> GetById(string id)
        {
            // users can get their own account and admins can get any account
            if (id != Account.Id.ToString() && Account.Role != Role.Admin)
            {
                return Unauthorized(new {message = "Unauthorized"});
            }

            var account = _accountService.GetById(id);
            return Ok(new ApiOkResponse(account));
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public ActionResult<AccountResponse> Create(CreateRequest model)
        {
            var account = _accountService.Create(model);
            return Ok(new ApiOkResponse(account));
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<AccountResponse> Update(string id, UpdateRequest model)
        {
            // users can update their own account and admins can update any account
            if (id != Account.Id.ToString() && Account.Role != Role.Admin)
            {
                return Unauthorized(new {message = "Unauthorized"});
            }

            // only admins can update role
            if (Account.Role != Role.Admin)
            {
                model.Role = null;
            }

            var account = _accountService.Update(id, model);
            return Ok(new ApiOkResponse(account));
        }

        // [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            // users can delete their own account and admins can delete any account
            //if (id != Account.Id && Account.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            _accountService.Delete(id);
            return Ok(new ApiOkResponse("Account deleted successfully"));
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }

        private string GetHostUrl()
        {
            var scheme = httpContextAccessor.HttpContext.Request.Scheme;
            var host = httpContextAccessor.HttpContext.Request.Host;
            var pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            var location = $"{scheme}://{host}{pathBase}";
            return location;
        }
    }
}
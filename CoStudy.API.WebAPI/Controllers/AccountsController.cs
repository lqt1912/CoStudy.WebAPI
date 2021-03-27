using AutoMapper;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class AccountController
    /// </summary>
    /// <seealso cref="CoStudy.API.WebAPI.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="accountService">The account service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public AccountsController(
            IAccountService accountService,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Authenticates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse response = _accountService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public ActionResult<AuthenticateResponse> RefreshToken()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            AuthenticateResponse response = _accountService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Revokes the token.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            string token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            _accountService.RevokeToken(token, ipAddress());
            return Ok(new ApiOkResponse("Token revoked"));
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterRequest model)
        //{
        //    await _accountService.Register(model, GetHostUrl());
        //    return Ok(new ApiOkResponse( "Registration successful, please check your email for verification instructions" ));
        //}

        /// <summary>
        /// Verifies the email.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(string token)
        {
            _accountService.VerifyEmail(token);
            return Ok(new ApiOkResponse("Verification successful, you can now login"));
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, GetHostUrl());

            return Ok(new ApiOkResponse("Please check your email for password reset instructions"));
        }

        /// <summary>
        /// Validates the reset token.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost("validate-reset-token")]
        public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
        {
            _accountService.ValidateResetToken(model);
            return Ok(new ApiOkResponse("Token is valid"));
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _accountService.ResetPassword(model);
            return Ok(new ApiOkResponse("Password reset successful, you can now login"));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<AccountResponse>> GetAll()
        {
            IEnumerable<AccountResponse> accounts = _accountService.GetAll();
            return Ok(new ApiOkResponse(accounts));
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<AccountResponse> GetById(string id)
        {
            // users can get their own account and admins can get any account
            if (id != Account.Id.ToString() && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            AccountResponse account = _accountService.GetById(id);
            return Ok(new ApiOkResponse(account));
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorize(Role.Admin)]
        [HttpPost]
        public ActionResult<AccountResponse> Create(CreateRequest model)
        {
            AccountResponse account = _accountService.Create(model);
            return Ok(new ApiOkResponse(account));
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<AccountResponse> Update(string id, UpdateRequest model)
        {
            // users can update their own account and admins can update any account
            if (id != Account.Id.ToString() && Account.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            // only admins can update role
            if (Account.Role != Role.Admin)
                model.Role = null;

            AccountResponse account = _accountService.Update(id, model);
            return Ok(new ApiOkResponse(account));
        }

        // [Authorize]
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the token cookie.
        /// </summary>
        /// <param name="token">The token.</param>
        private void setTokenCookie(string token)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        /// <summary>
        /// Ips the address.
        /// </summary>
        /// <returns></returns>
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Gets the host URL.
        /// </summary>
        /// <returns></returns>
        private string GetHostUrl()
        {
            string scheme = httpContextAccessor.HttpContext.Request.Scheme;
            HostString host = httpContextAccessor.HttpContext.Request.Host;
            PathString pathBase = httpContextAccessor.HttpContext.Request.PathBase;
            string location = $"{scheme}://{host}{pathBase}";
            return location;
        }
    }
}

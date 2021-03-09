using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Identity.Services.AccountService
{
    /// <summary>
    /// The Interface IAccountService
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        /// <summary>
        /// Revokes the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ipAddress">The ip address.</param>
        void RevokeToken(string token, string ipAddress);
        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        Task Register(RegisterRequest model, string origin);
        /// <summary>
        /// Verifies the email.
        /// </summary>
        /// <param name="token">The token.</param>
        void VerifyEmail(string token);
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        /// <summary>
        /// Validates the reset token.
        /// </summary>
        /// <param name="model">The model.</param>
        void ValidateResetToken(ValidateResetTokenRequest model);
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        void ResetPassword(ResetPasswordRequest model);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AccountResponse> GetAll();
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        AccountResponse GetById(string id);
        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        AccountResponse Create(CreateRequest model);
        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        AccountResponse Update(string id, UpdateRequest model);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(string id);

        /// <summary>
        /// Gets the current refresh token.
        /// </summary>
        /// <returns></returns>
        Task<string> GetCurrentRefreshToken();
    }
}

using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Identity.Services.AccountService
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        Task  RevokeToken(string token, string ipAddress);
        Task Register(RegisterRequest model, string origin);
        void VerifyEmail(string token);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(ValidateResetTokenRequest model);
        void ResetPassword(ResetPasswordRequest model);
        IEnumerable<AccountResponse> GetAll();
        AccountResponse GetById(string id);
        AccountResponse Create(CreateRequest model);
        AccountResponse Update(string id, UpdateRequest model);
        void Delete(string id);

        Task<string> GetCurrentRefreshToken();
        string generateJwtToken(Account account);
        RefreshToken generateRefreshToken(string ipAddress);
        void removeOldRefreshTokens(Account account);
    }
}

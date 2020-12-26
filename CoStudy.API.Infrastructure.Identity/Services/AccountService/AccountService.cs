using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace CoStudy.API.Infrastructure.Identity.Services.AccountService
{
    public class AccountService : IAccountService
    {
        IAccountRepository accountRepository;
        IMapper mapper;
        AppSettings appSettings;
        IEmailService emailService;
        IUserRepository userRepository;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
            this.userRepository = userRepository;
        }

        private string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("_id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = Extension.randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var cachedAccount = CacheHelper.GetValue($"CurrentAccount-{model.Email}") as Account;
            if (cachedAccount != null)
            {
                if (cachedAccount.Email == model.Email && BC.Verify(model.Password, cachedAccount.PasswordHash))
                {
                    var jwtToken1 = generateJwtToken(cachedAccount);
                    var refreshToken1 = generateRefreshToken(ipAddress);
                    cachedAccount.RefreshTokens.Add(refreshToken1);
                    removeOldRefreshTokens(cachedAccount);

                    accountRepository.Update(cachedAccount, cachedAccount.Id);

                    var response1 = mapper.Map<AuthenticateResponse>(cachedAccount);
                    response1.JwtToken = jwtToken1;
                    response1.RefreshToken = refreshToken1.Token;
                    return response1;
                }
            }

            var account = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);
            if (account == null || !account.IsVerified)
                throw new Exception("Email or password incorrect");

            if (!BC.Verify(model.Password, account.PasswordHash))
                throw new Exception("Email or password incorrect");

            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);
            removeOldRefreshTokens(account);

            accountRepository.Update(account, account.Id);

            var response = mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;

            var currentUser = userRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);

            CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));
            CacheHelper.Add($"CurrentAccount-{account.Email}", account, DateTime.Now.AddDays(10));

            return response;
        }

        public AccountResponse Create(CreateRequest model)
        {
            var currentAccount = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);
            if (currentAccount != null)
                throw new Exception($"Email {model.Email} has been registered");

            var account = mapper.Map<Account>(model);
            account.Created = DateTime.UtcNow;
            account.Verified = DateTime.UtcNow;
            account.PasswordHash = BC.HashPassword(model.Password);
            accountRepository.Add(account);
            return mapper.Map<AccountResponse>(account);
        }

        public void Delete(string id)
        {
            accountRepository.Delete(ObjectId.Parse(id));
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {

            var account = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            // create reset token that expires after 1 day
            account.ResetToken = Extension.randomOTPString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            accountRepository.Update(account, account.Id);

            // send email
            await sendPasswordResetEmail(account, origin);


        }

        public IEnumerable<AccountResponse> GetAll()
        {
            var accounts = accountRepository.GetAll();
            return mapper.Map<IList<AccountResponse>>(accounts);
        }



        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(account);

            accountRepository.Update(account, account.Id);

            // generate new jwt
            var jwtToken = generateJwtToken(account);

            var response = mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        public async Task Register(RegisterRequest model, string origin)
        {
            if (accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email) != null)
            {
                await sendAlreadyRegisteredEmail(model.Email, origin);
                return;
            }
            var account = mapper.Map<Account>(model);

            var isFirstAccount = accountRepository.GetAll().Count() == 0;
            account.Role = isFirstAccount ? Role.Admin : Role.User;
            account.Created = DateTime.UtcNow;
            account.VerificationToken = Extension.randomOTPString();

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            accountRepository.Add(account);

            await sendVerificationEmail(account, origin);
        }

        public void ResetPassword(ResetPasswordRequest model)
        {

            var account = accountRepository.GetAll().SingleOrDefault(
                x => x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow
                );
            if (account == null)
                throw new AppException("Invalid token");

            // update password and remove reset token
            account.PasswordHash = BC.HashPassword(model.Password);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            accountRepository.Update(account, account.Id);

        }

        public void RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            accountRepository.Update(account, account.Id);
        }

        public AccountResponse Update(string id, UpdateRequest model)
        {
            var account = accountRepository.GetById(ObjectId.Parse(id));

            if (account.Email != model.Email
                && accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email) != null)
                throw new NotImplementedException();
            {
                throw new Exception($"Email {model.Email} has been taken");
            }

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                account.PasswordHash = BC.HashPassword(model.Password);

            // copy model to account and save
            mapper.Map(model, account);
            account.Updated = DateTime.UtcNow;
            accountRepository.Update(account, account.Id);

            return mapper.Map<AccountResponse>(account);
        }

        public void ValidateResetToken(ValidateResetTokenRequest model)
        {

            var account = accountRepository.GetAll().SingleOrDefault(x => x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);
            if (account == null)
                throw new Exception("Invalid Token! ");
        }

        public void VerifyEmail(string token)
        {

            var account = accountRepository.GetAll().SingleOrDefault(x => x.VerificationToken == token);

            if (account == null)
                throw new Exception("Verfication failed! ");
            account.Verified = DateTime.Now;
            account.VerificationToken = null;
            accountRepository.Update(account, account.Id);
        }


        private async Task sendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }


            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = account.Email,
                Subject = "Đăng kí tài khoản CoStudy - đặt lại mật khẩu",
                Body = $@"<h4>Reset Password Email</h4>
                         {message}"
            };

            await emailService.SendEmailAsync(mailRequest);
        }

        private async Task sendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/api/Accounts/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            //_emailService.Send(
            //    to: account.Email,
            //    subject: "Sign-up Verification API - Verify Email",
            //    html: 
            // );
            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = account.Email,
                Subject = "Mã kích hoạt tài khoản CoStudy",
                Body = $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            };
            await emailService.SendEmailAsync(mailRequest);
        }

        private async Task sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );

            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = email,
                Subject = "Đăng kí tài khoản CoStudy - Email đã bị tái đăng kí",
                Body = $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            };

            await emailService.SendEmailAsync(mailRequest);
        }

        private (RefreshToken, Account) getRefreshToken(string token)
        {
            var account = accountRepository.GetAll().SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        public AccountResponse GetById(string id)
        {
            var account = accountRepository.GetById(ObjectId.Parse(id));
            return mapper.Map<AccountResponse>(account);
        }
    }
}

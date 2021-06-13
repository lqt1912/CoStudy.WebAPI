using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Models.Account.Request;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.AccessControl;
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

        IHttpContextAccessor httpContextAccessor;
        public AccountService(IAccountRepository accountRepository, IMapper mapper, IOptions<AppSettings> appSettings,
            IEmailService emailService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("_id", account.Id.ToString()),
                    new Claim("_email", account.Email)

                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = Extension.randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var emailBuidler = Builders<Account>.Filter;
            var emailFilter = emailBuidler.Eq("Email", model.Email);
            var account = await accountRepository.FindAsync((emailFilter));
            if (account == null)
            {
                var userEmailFilter = Builders<User>.Filter.Eq("phone_number", model.Email);
                var user = await userRepository.FindAsync(userEmailFilter);
                if(user ==null)
                    throw new Exception("Tài khoản hoặc mật khẩu không đúng. ");
                emailFilter = emailBuidler.Eq("Email", user.Email);
                account = await accountRepository.FindAsync((emailFilter));
            }
            if (!BC.Verify(model.Password, account.PasswordHash))
                throw new Exception("Tài khoản hoặc mật khẩu không đúng. ");

            if (!account.IsVerified)
                throw new Exception("Tài khoản chưa được xác thực. ");

            var _user = await userRepository.FindAsync(Builders<User>.Filter.Eq("email", account.Email));

            var existJwts = _user.JwtTokens;

            foreach (var jwt in _user.JwtTokens.ToList().Where(jwt => Feature.ValidateToken(jwt, appSettings.Secret) ==null))
            {
                _user.JwtTokens.Remove(jwt);
            }

            var refreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);
            removeOldRefreshTokens(account);
            var response = mapper.Map<AuthenticateResponse>(account);
            response.RefreshToken = refreshToken.Token;
            _user.LatestRefreshToken = refreshToken.Token;
            if (_user.JwtTokens.Any())
            {
                response.JwtToken = _user.JwtTokens.ElementAt(0);
            }
            else
            {
                var jwtToken = generateJwtToken(account);
                response.JwtToken = jwtToken;
                _user.JwtTokens.Add(jwtToken);
            }
            await accountRepository.UpdateAsync(account, account.Id);
            await userRepository.UpdateAsync(_user, _user.Id);

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

            await accountRepository.UpdateAsync(account, account.Id);

            // send email
            await sendPasswordResetEmail(account, origin);
        }

        public IEnumerable<AccountResponse> GetAll()
        {
            var accounts = accountRepository.GetAll();
            return mapper.Map<IList<AccountResponse>>(accounts);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
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

            var _user = await userRepository.FindAsync(Builders<User>.Filter.Eq("email", account.Email));
            _user.LatestRefreshToken = response.RefreshToken;
            _user.JwtTokens.Add(jwtToken);
            await userRepository.UpdateAsync(_user, _user.Id);

            return response;
        }

        public async Task Register(RegisterRequest model, string origin)
        {
            if (accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email) != null)
            {
                await sendAlreadyRegisteredEmail(model.Email, origin);
                throw new Exception($"Email {model.Email} has been registered");
            }

            var account = mapper.Map<Account>(model);

            var isFirstAccount = !accountRepository.GetAll().Any();
            account.Role = isFirstAccount ? Role.Admin : Role.User;
            account.Created = DateTime.UtcNow;
            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            await accountRepository.AddAsync(account);

            if (model.IsExternalRegister == true)
            {
                account.VerificationToken = string.Empty;
                account.Verified = DateTime.Now;
                await accountRepository.UpdateAsync(account, account.Id);
                return;
            }
            account.VerificationToken = Extension.randomOTPString();
            await accountRepository.UpdateAsync(account, account.Id);
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

        public async Task RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);
            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            var _user = await userRepository.FindAsync(Builders<User>.Filter.Eq("email", account.Email));
            _user.LatestRefreshToken =string.Empty;
            await userRepository.UpdateAsync(_user, _user.Id);

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
            var account = accountRepository.GetAll()
                .SingleOrDefault(x => x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);
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

        public async Task sendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message =
                    $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message =
                    $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }


            var mailRequest = new MailRequest()
            {
                ToEmail = account.Email,
                Subject = "Đăng kí tài khoản CoStudy - đặt lại mật khẩu",
                Body = $@"<h4>Reset Password Email</h4>
                         {message}"
            };

            await emailService.SendEmailAsync(mailRequest);
        }

        public async Task sendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                message = $@"<p>Mã kích hoạt của bạn là: {account.VerificationToken}</p>";
            }
            else
            {
                message =
                    $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            //_emailService.Send(
            //    to: account.Email,
            //    subject: "Sign-up Verification API - Verify Email",
            //    html: 
            // );
            var mailRequest = new MailRequest()
            {
                ToEmail = account.Email,
                Subject = "Mã kích hoạt tài khoản CoStudy",
                Body = $@"<h4>Xác thực thông tin email</h4>
                         <p>Cảm ơn bạn đã đăng kí sử dụng dịch vụ!</p>
                         {message}"
            };
            await emailService.SendEmailAsync(mailRequest);
        }

        public async Task sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message =
                    $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message =
                    "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );

            var mailRequest = new MailRequest()
            {
                ToEmail = email,
                Subject = "Đăng kí tài khoản CoStudy - Email đã bị tái đăng kí",
                Body = $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            };

            await emailService.SendEmailAsync(mailRequest);
        }

        public (RefreshToken, Account) getRefreshToken(string token)
        {
            var account = accountRepository.GetAll()
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

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

        public async Task<string> GetCurrentRefreshToken()
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var filter = Builders<Account>.Filter.Eq("Email", currentUser.Email);
            var currentAccount = await accountRepository.FindAsync(filter);
            if (currentAccount != null)
            {
                var refreshTokens =
                    currentAccount.RefreshTokens.OrderByDescending(x => x.Created);
                var latestRefresh = refreshTokens.ElementAt(0);
                if (!latestRefresh.IsExpired)
                    return latestRefresh.Token;
                else throw new Exception("Có lỗi xảy ra");
            }
            else throw new Exception("Có lỗi xảy ra");
        }
    }
}
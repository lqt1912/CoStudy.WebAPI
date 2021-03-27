using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace CoStudy.API.Infrastructure.Identity.Services.AccountService
{
    /// <summary>
    /// Class AccountService
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Identity.Services.AccountService.IAccountService" />
    public class AccountService : IAccountService
    {
        /// <summary>
        /// The account repository
        /// </summary>
        IAccountRepository accountRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// The application settings
        /// </summary>
        AppSettings appSettings;

        /// <summary>
        /// The email service
        /// </summary>
        IEmailService emailService;

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        IFcmRepository fcmRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public AccountService(IAccountRepository accountRepository, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IFcmRepository fcmRepository)
        {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
            this.emailService = emailService;
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.fcmRepository = fcmRepository;
        }

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public string generateJwtToken(Account account)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("_id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates the refresh token.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes the old refresh tokens.
        /// </summary>
        /// <param name="account">The account.</param>
        public void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        /// <summary>
        /// Authenticates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Email or password incorrect
        /// or
        /// Email is not verified
        /// or
        /// Email or password incorrect
        /// </exception>
        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            Account account = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);
            if (account == null)
                throw new Exception("Email or password incorrect");

            if (!account.IsVerified)
                throw new Exception("Email is not verified");

            if (!BC.Verify(model.Password, account.PasswordHash))
                throw new Exception("Email or password incorrect");

            string jwtToken = generateJwtToken(account);
            RefreshToken refreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);
            removeOldRefreshTokens(account);

            accountRepository.Update(account, account.Id);

            AuthenticateResponse response = mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;

            Domain.Entities.Application.User currentUser = userRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);

            CacheHelper.Add($"CurrentUser-{currentUser.Email}", currentUser, DateTime.Now.AddDays(10));
            CacheHelper.Add($"CurrentAccount-{account.Email}", account, DateTime.Now.AddDays(10));

            fcmRepository.AddToGroup(new AddUserToGroupRequest()
            {
                UserIds = new List<string> { currentUser.OId },
                GroupName = currentUser.OId,
                Type = Feature.GetTypeName(currentUser)

            });

            return response;
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Email {model.Email} has been registered</exception>
        public AccountResponse Create(CreateRequest model)
        {
            Account currentAccount = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);
            if (currentAccount != null)
                throw new Exception($"Email {model.Email} has been registered");

            Account account = mapper.Map<Account>(model);
            account.Created = DateTime.UtcNow;
            account.Verified = DateTime.UtcNow;
            account.PasswordHash = BC.HashPassword(model.Password);
            accountRepository.Add(account);
            return mapper.Map<AccountResponse>(account);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(string id)
        {
            accountRepository.Delete(ObjectId.Parse(id));
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="origin">The origin.</param>
        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {

            Account account = accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            // create reset token that expires after 1 day
            account.ResetToken = Extension.randomOTPString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            accountRepository.Update(account, account.Id);

            // send email
            await sendPasswordResetEmail(account, origin);


        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountResponse> GetAll()
        {
            IQueryable<Account> accounts = accountRepository.GetAll();
            return mapper.Map<IList<AccountResponse>>(accounts);
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            (RefreshToken refreshToken, Account account) = getRefreshToken(token);

            // replace old refresh token with a new one and save
            RefreshToken newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(account);

            accountRepository.Update(account, account.Id);

            // generate new jwt
            string jwtToken = generateJwtToken(account);

            AuthenticateResponse response = mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="origin">The origin.</param>
        /// <exception cref="Exception">Email {model.Email} has been registered</exception>
        public async Task Register(RegisterRequest model, string origin)
        {
            if (accountRepository.GetAll().SingleOrDefault(x => x.Email == model.Email) != null)
            {
                await sendAlreadyRegisteredEmail(model.Email, origin);
                throw new Exception($"Email {model.Email} has been registered");
            }
            Account account = mapper.Map<Account>(model);

            bool isFirstAccount = accountRepository.GetAll().Count() == 0;
            account.Role = isFirstAccount ? Role.Admin : Role.User;
            account.Created = DateTime.UtcNow;
            account.VerificationToken = Extension.randomOTPString();

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            accountRepository.Add(account);

            await sendVerificationEmail(account, origin);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="AppException">Invalid token</exception>
        public void ResetPassword(ResetPasswordRequest model)
        {

            Account account = accountRepository.GetAll().SingleOrDefault(
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

        /// <summary>
        /// Revokes the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="ipAddress">The ip address.</param>
        public void RevokeToken(string token, string ipAddress)
        {
            (RefreshToken refreshToken, Account account) = getRefreshToken(token);
            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            accountRepository.Update(account, account.Id);
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception">Email {model.Email} has been taken</exception>
        public AccountResponse Update(string id, UpdateRequest model)
        {
            Account account = accountRepository.GetById(ObjectId.Parse(id));

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

        /// <summary>
        /// Validates the reset token.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="Exception">Invalid Token!</exception>
        public void ValidateResetToken(ValidateResetTokenRequest model)
        {

            Account account = accountRepository.GetAll().SingleOrDefault(x => x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);
            if (account == null)
                throw new Exception("Invalid Token! ");
        }

        /// <summary>
        /// Verifies the email.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="Exception">Verfication failed!</exception>
        public void VerifyEmail(string token)
        {

            Account account = accountRepository.GetAll().SingleOrDefault(x => x.VerificationToken == token);

            if (account == null)
                throw new Exception("Verfication failed! ");
            account.Verified = DateTime.Now;
            account.VerificationToken = null;
            accountRepository.Update(account, account.Id);
        }

        /// <summary>
        /// Sends the password reset email.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="origin">The origin.</param>
        public async Task sendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                string resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
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

        /// <summary>
        /// Sends the verification email.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="origin">The origin.</param>
        public async Task sendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                message = $@"<p>Registeration token: {account.VerificationToken}</p>";
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

        /// <summary>
        /// Sends the already registered email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="origin">The origin.</param>
        public async Task sendAlreadyRegisteredEmail(string email, string origin)
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

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="AppException">
        /// Invalid token
        /// or
        /// Invalid token
        /// </exception>
        public (RefreshToken, Account) getRefreshToken(string token)
        {
            Account account = accountRepository.GetAll().SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (account == null) throw new AppException("Invalid token");

            RefreshToken refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive) throw new AppException("Invalid token");

            return (refreshToken, account);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public AccountResponse GetById(string id)
        {
            Account account = accountRepository.GetById(ObjectId.Parse(id));
            return mapper.Map<AccountResponse>(account);
        }

        /// <summary>
        /// Gets the current refresh token.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Có lỗi xảy ra
        /// or
        /// Có lỗi xảy ra
        /// </exception>
        public async Task<string> GetCurrentRefreshToken()
        {
            Domain.Entities.Application.User currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            FilterDefinition<Account> filter = Builders<Account>.Filter.Eq("Email", currentUser.Email);
            Account currentAccount = await accountRepository.FindAsync(filter);
            if (currentAccount != null)
            {
                IOrderedEnumerable<RefreshToken> refreshTokens = currentAccount.RefreshTokens.OrderByDescending(x => x.Created);
                RefreshToken latestRefresh = refreshTokens.ElementAt(0);
                if (!latestRefresh.IsExpired)
                    return latestRefresh.Token;
                else throw new Exception("Có lỗi xảy ra");
            }
            else throw new Exception("Có lỗi xảy ra");
        }
    }
}

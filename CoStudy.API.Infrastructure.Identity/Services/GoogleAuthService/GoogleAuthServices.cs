using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace CoStudy.API.Infrastructure.Identity.Services.GoogleAuthService
{
    /// <summary>
    /// Class GoogleAuthServices
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Identity.Services.GoogleAuthService.IGoogleAuthServices" />
    public class GoogleAuthServices :IGoogleAuthServices
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// The user repository
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// The external login repository
        /// </summary>
        IExternalLoginRepository externalLoginRepository;

        /// <summary>
        /// The account service
        /// </summary>
        IAccountService accountService;
        /// <summary>
        /// The account repository
        /// </summary>
        IAccountRepository accountRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAuthServices" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="externalLoginRepository">The external login repository.</param>
        /// <param name="accountService">The account service.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="mapper">The mapper.</param>
        public GoogleAuthServices(IConfiguration configuration, IUserRepository userRepository, IExternalLoginRepository externalLoginRepository, IAccountService accountService, IAccountRepository accountRepository, IMapper mapper)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.externalLoginRepository = externalLoginRepository;
            this.accountService = accountService;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Verifies the google token.
        /// </summary>
        /// <param name="externalAuth">The external authentication.</param>
        /// <returns></returns>
        public async Task<Payload> VerifyGoogleToken(GoogleAuth externalAuth)
        {
            try
            {
                var settings = new ValidationSettings()
                {
                    Audience = new List<string>() { configuration["clientId"] }
                };

                var payload = await ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }


        /// <summary>
        /// Externals the login.
        /// </summary>
        /// <param name="externalAuth">The external authentication.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unauthorized
        /// or
        /// Vui lòng đăng kí thông tin.</exception>
        public async Task<AuthenticateResponse> ExternalLogin(GoogleAuth externalAuth, string ipAddress)
        {
            var payLoad = await VerifyGoogleToken(externalAuth);

            if (payLoad == null)
                throw new Exception("Unauthorized");

            var filter = Builders<User>.Filter;

            var finder = filter.Eq("email", payLoad.Email);

            var internalUser = await userRepository.FindAsync(finder);

            if(internalUser !=null)
            {
                var externalFilter = Builders<ExternalLogin>.Filter.Eq("user_id", internalUser.OId);

                var existExternalLogin = await externalLoginRepository.FindAsync(externalFilter);

                //Chưa có External
                if (existExternalLogin == null)
                {
                    ExternalLogin external = new ExternalLogin()
                    {
                        LoginProvider = externalAuth.Provider,
                        ProviderDisplayName = externalAuth.Provider,
                        UserId = internalUser.OId
                    };

                    await externalLoginRepository.AddAsync(external);
                }

                Account account = accountRepository.GetAll().FirstOrDefault(x => x.Email == internalUser.Email);

                string jwtToken = accountService.generateJwtToken(account);
                RefreshToken refreshToken = accountService.generateRefreshToken(ipAddress);
                account.RefreshTokens.Add(refreshToken);
                accountService.removeOldRefreshTokens(account);

                accountRepository.Update(account, account.Id);

                AuthenticateResponse response = mapper.Map<AuthenticateResponse>(account);
                response.JwtToken = jwtToken;
                response.RefreshToken = refreshToken.Token;
                return response;
            }
            else //Chưa có internal
            {
                ExternalLogin external = new ExternalLogin()
                {
                    LoginProvider = externalAuth.Provider,
                    ProviderDisplayName = externalAuth.Provider,
                    UserId = internalUser.OId
                };

                await externalLoginRepository.AddAsync(external);

                //Nhảy về giao diện điền thông tin. 
                throw new Exception("Vui lòng đăng kí thông tin. ");
            }
        }
    }
}

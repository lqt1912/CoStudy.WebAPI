using AutoMapper;
using Common;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.Account.Response;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
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
    public class GoogleAuthServices : IGoogleAuthServices
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
        /// Externals the login.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public async Task<object> ExternalLogin(GoogleAuthenticationRequest request, string ipAddress)
        {
            var internalUserFilter = Builders<User>.Filter.Eq("email", request.User.Email);
            var existInternalUser = await userRepository.FindAsync(internalUserFilter);
            if (existInternalUser != null)
            {
                var finder = Builders<ExternalLogin>.Filter.Eq("email", request.User.Email);

                var existExternalLogin = await externalLoginRepository.FindAsync(finder);

                if (existExternalLogin == null)
                {
                    var externalLogin = new ExternalLogin()
                    {
                        Email = request.User.Email,
                        LoginProvider = Constants.ExternalLoginConstants.GOOGLE_EXTERNAL_PROVIDER
                    };

                    await externalLoginRepository.AddAsync(externalLogin);
                }

                Account account = accountRepository.GetAll().FirstOrDefault(x => x.Email == existExternalLogin.Email);

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
            else
            {
                return request;
            }
        }
    }
}

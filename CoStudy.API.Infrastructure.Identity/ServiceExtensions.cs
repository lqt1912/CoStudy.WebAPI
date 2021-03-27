using AutoMapper;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Identity.Services.GoogleAuthService;
using CoStudy.API.Infrastructure.Identity.Services.Implements;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoStudy.API.Infrastructure.Identity
{
    /// <summary>
    /// Class Services Extensions
    /// </summary>
    public static class ServiceExtensions
    {

        /// <summary>
        /// Configures the identity.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IExternalLoginRepository, ExternalLoginRepository>();
            services.AddTransient<IGoogleAuthServices, GoogleAuthServices>();
        }
    }
}

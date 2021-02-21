using AutoMapper;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using CoStudy.API.Infrastructure.Identity.Services.AccountService;
using CoStudy.API.Infrastructure.Identity.Services.Implements;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoStudy.API.Infrastructure.Identity
{
    public static class ServiceExtensions
    {

        public static void ConfigureIdentity(this IServiceCollection services)
        {

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();

            //services.Configure<AuthMessageSenderOptions>(option =>
            //{
            //    option.SendGridUser = "Costudy.API";
            //    option.SendGridKey = "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";
            //});

        }
    }
}

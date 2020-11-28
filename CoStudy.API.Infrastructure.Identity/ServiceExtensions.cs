using AutoMapper;
using CoStudy.API.Infrastructure.Identity.Contexts;
using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Services.Implements;
using CoStudy.API.Infrastructure.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CoStudy.API.Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<AuthMessageSenderOptions>(option =>
            {
                option.SendGridUser = "CoSudy.API";
                option.SendGridKey = "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";
            });
        }
    }
}

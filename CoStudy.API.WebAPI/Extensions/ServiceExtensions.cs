using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System;
using System.Collections.Generic;

namespace CoStudy.API.WebAPI.Extensions
{
    public static class ServiceExtensions
    {

        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CoStudy's API",
                    Description = "Study Communication system backend API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Le Quoc Thang",
                        Email = "thang.le@tekmedi.com",
                        Url = new Uri("https://github.com/lqt1912"),
                    }
                });

                // 🔐 JWT Bearer definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Nhập token theo dạng: Bearer {token}"
                });

                // 🔒 Apply JWT globally
                c.AddSecurityRequirement( _ => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference(SecuritySchemeType.Http.ToString()),
                        new List<string>()
                    }   
                });
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors();

        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
                options.MaximumBodySize = 1024;
            });
        }

        public static void ConfigureApiAuthentication(this IServiceCollection services)
        {

        }

        public static void ConfigIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimit"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}

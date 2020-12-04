using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

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
                    Title = "CoStudy's api",
                    Description= "Study Communication system backend API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Le Quoc Thang",
                        Email = "thang.le@tekmedi.com",
                        Url = new Uri("https://github.com/lqt1912"),
                    },
                });
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                   builder => builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            });
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
    }
}

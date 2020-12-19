using CoStudy.API.Domain.Entities.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public static class SignalRConfig
    {
        public static void SignalRConfigs(this IServiceCollection services)
        {
           // services.AddAuthentication(options =>
           // {
           //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           // })
           //.AddJwtBearer(options =>
           //{
           //    options.Events = new JwtBearerEvents
           //    {
           //        OnMessageReceived = context =>
           //        {
           //            StringValues accessToken = context.Request.Query["Authorization"];
           //            PathString path = context.HttpContext.Request.Path;

           //            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/messagehub"))
           //            {
           //                context.Token = accessToken;
           //            }
           //            return Task.CompletedTask;
           //        }
           //    };
           //});
            services.AddSignalR();
        }

        public static void SignalRConfigs(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
               endpoints.MapHub<BaseHub<Message>>("/messagehub");
            });
        }
    }
}

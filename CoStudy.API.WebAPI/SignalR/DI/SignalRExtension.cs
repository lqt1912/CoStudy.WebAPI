using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using CoStudy.API.WebAPI.SignalR.DI.Message;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR.DI
{
    public static  class SignalRExtension
    {
        public static void ConfigureSignalRHub(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddTransient<IMessageHub, MessageHub>();

        }

        public static void COnfigureSignalrApp(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalRHub<AddMessageResponse>>("/messagehub");
            });
        }
    }
}

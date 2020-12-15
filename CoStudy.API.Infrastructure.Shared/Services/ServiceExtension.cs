﻿using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public static class ServiceExtension
    {
        public static void RegisterCustomService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IPostService, PostService>();
            services.AddTransient<INofticationService, NofticationService>();
        }
    }
}

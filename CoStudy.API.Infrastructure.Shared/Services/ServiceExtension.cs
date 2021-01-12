using CoStudy.API.Infrastructure.Shared.Services.LocationServices;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public static class ServiceExtension
    {
        public static void RegisterCustomService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<INofticationService, NofticationService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ILocationService, LocationService>();
        }
    }
}

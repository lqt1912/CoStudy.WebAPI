using AutoMapper;
using CoStudy.API.Application.FCM;
using CoStudy.API.Infrastructure.Shared.AutoMapper;
using CoStudy.API.Infrastructure.Shared.Services.LocationServices;
using CoStudy.API.Infrastructure.Shared.Services.MessageServices;
using CoStudy.API.Infrastructure.Shared.Services.NofticationServices;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Service extension
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Registers the custom service.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void RegisterCustomService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<INofticationService, NofticationService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<ILevelService, LevelService>();
            services.AddTransient<IDocumentServices, DocumentServices>();
            services.AddTransient<IReportServices, ReportServices>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IReportReasonService, ReportReasonService>();
            services.AddTransient<ILoggingServices, LoggingServices>();
            services.AddTransient<IConversationService, ConversationService>();
            services.AddTransient<IFcmRepository, FcmRepository>();
            services.AddTransient<ICmsServices, CmsServices>();
            services.AddTransient<IFieldServices, FieldServices>();
        }
    }
}

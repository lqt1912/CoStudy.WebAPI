using CoStudy.API.Application.FCM;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;

namespace CoStudy.API.Application.Repositories
{
    public static class RepositoryRegistration
    {

        public static void RegisterCustomRepository(this IServiceCollection services)
        {
            services.AddTransient<IAdditionalInfoRepository, AdditionalInfoRepository>();

            services.AddTransient<IAddressRepository, AddressRepository>();

            services.AddTransient<ICommentRepository, CommentRepository>();

            services.AddTransient<IConversationRepository, ConversationRepository>();

            services.AddTransient<IFieldRepository, FieldRepository>();

            services.AddTransient<IImageRepository, ImageRepository>();

            services.AddTransient<IMediaContentRepository, MediaContentRepository>();

            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddTransient<INofticationRepository, NofticationRepository>();

            services.AddTransient<IPostRepository, PostRepository>();

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IReplyCommentRepository, ReplyCommentRepository>();

            services.AddTransient<IClientGroupRepository, ClientGroupRepository>();


            services.AddTransient<IClientGroupRepository, ClientGroupRepository>();

            services.AddTransient<ILoggingRepository, LoggingRepository>();

            services.AddTransient<IFollowRepository, FollowRepository>();

            services.AddTransient<IDownVoteRepository, DownVoteRepository>();

            services.AddTransient<IUpVoteRepository, UpVoteRepository>();

            services.AddTransient<IFcmInfoRepository, FcmInfoRepository>();

            services.AddTransient<IFcmRepository, FcmRepository>();
            services.AddTransient<IProvinceRepository, ProvinceRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IWardRepository, WardRepository>();

            var googleCredential = GoogleCredential.FromFile(@"wwwroot/costudy-c5390-firebase-adminsdk-e63r1-4405752328.json");
            FirebaseApp.Create(new AppOptions() { Credential = googleCredential });
        }

    }
}

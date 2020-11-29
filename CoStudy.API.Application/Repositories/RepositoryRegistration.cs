using CoStudy.API.Domain.Entities.Application;
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

        }

    }
}

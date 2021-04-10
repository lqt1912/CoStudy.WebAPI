using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessagePostThumbnailRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MessagePostThumbnail}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessagePostThumbnailRepository" />
    public class MessagePostThumbnailRepository : BaseRepository<MessagePostThumbnail>, IMessagePostThumbnailRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePostThumbnailRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessagePostThumbnailRepository(IConfiguration configuration) : base("message_postthumbnail", configuration)
        {
            this.configuration = configuration;
        }
    }
}

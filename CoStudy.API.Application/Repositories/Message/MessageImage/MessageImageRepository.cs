using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessageImageRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MessageImage}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessageImageRepository" />
    public class MessageImageRepository : BaseRepository<MessageImage>, IMessageImageRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageImageRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageImageRepository(IConfiguration configuration) : base("message_image", configuration)
        {
            this.configuration = configuration;
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessageRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Message}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessageRepository" />
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageRepository(IConfiguration configuration) : base("message", configuration)
        {
            this.configuration = configuration;
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessageTextRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MessageText}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessageTextRepository" />
    public class MessageTextRepository : BaseRepository<MessageText>, IMessageTextRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTextRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageTextRepository(IConfiguration configuration) : base("message_text", configuration)
        {
            this.configuration = configuration;
        }
    }
}

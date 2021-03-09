using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class ConversationRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Conversation}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IConversationRepository" />
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ConversationRepository(IConfiguration configuration) : base("conversation", configuration)
        {
            this.configuration = configuration;
        }
    }
}

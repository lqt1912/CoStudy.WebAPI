using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class ConversationItemTypeRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.ConversationItemType}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IConversationItemTypeRepository" />
    public class ConversationItemTypeRepository : BaseRepository<ConversationItemType>, IConversationItemTypeRepository
    {

        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationItemTypeRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ConversationItemTypeRepository(IConfiguration configuration) : base("conversation_item_type", configuration)
        {
            this.configuration = configuration;
        }
    }
}

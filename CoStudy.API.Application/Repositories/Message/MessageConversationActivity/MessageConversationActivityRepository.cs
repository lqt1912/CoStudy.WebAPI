using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessageConversationActivityRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MessageConversationActivity}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessageConversationActivityRepository" />
    public class MessageConversationActivityRepository : BaseRepository<MessageConversationActivity>, IMessageConversationActivityRepository
    {
        IConfiguration configuration;

        public MessageConversationActivityRepository(IConfiguration configuration) : base("message_conversation", configuration)
        {
            this.configuration = configuration;
        }
    }
}

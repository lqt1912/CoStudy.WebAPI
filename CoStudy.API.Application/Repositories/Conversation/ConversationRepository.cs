using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        IConfiguration configuration;
        public ConversationRepository(IConfiguration configuration) : base("conversation", configuration)
        {
            this.configuration = configuration;
        }
    }
}

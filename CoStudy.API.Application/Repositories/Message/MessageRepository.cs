using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        IConfiguration configuration;
        public MessageRepository(IConfiguration configuration) : base("message", configuration)
        {
            this.configuration = configuration;
        }
    }
}

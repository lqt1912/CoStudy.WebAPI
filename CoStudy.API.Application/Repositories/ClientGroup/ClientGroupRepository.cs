using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class ClientGroupRepository : BaseRepository<ClientGroup>, IClientGroupRepository
    {
        IConfiguration configuration;
        public ClientGroupRepository(IConfiguration configuration) : base("client_group", configuration)
        {
            this.configuration = configuration;
        }
    }
}

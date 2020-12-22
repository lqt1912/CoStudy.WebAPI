using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class ClientConnectionsRepository : BaseRepository<ClientConnections>, IClientConnectionsRepository
    {
        public ClientConnectionsRepository() : base("client_connections")
        {

        }
    }
}

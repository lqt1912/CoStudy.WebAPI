using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class ClientGroupRepository : BaseRepository<ClientGroup>, IClientGroupRepository
    {
        public ClientGroupRepository() : base("client_group")
        {

        }
    }
}

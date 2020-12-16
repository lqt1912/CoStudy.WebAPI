using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class NofticationRepository : BaseRepository<Noftication>, INofticationRepository
    {
        public NofticationRepository() : base("noftication")
        {

        }
    }
}

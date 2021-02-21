using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class NofticationRepository : BaseRepository<Noftication>, INofticationRepository
    {
        IConfiguration configuration;
        public NofticationRepository(IConfiguration configuration) : base("noftication", configuration)
        {
            this.configuration = configuration;
        }
    }
}

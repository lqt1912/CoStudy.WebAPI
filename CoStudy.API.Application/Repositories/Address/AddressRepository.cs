using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        IConfiguration configuration;
        public AddressRepository(IConfiguration configuration) : base("address", configuration)
        {
            this.configuration = configuration;
        }
    }
}

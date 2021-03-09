using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class AddressRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Address}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IAddressRepository" />
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AddressRepository(IConfiguration configuration) : base("address", configuration)
        {
            this.configuration = configuration;
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class AdditionalInfoRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.AdditionalInfo}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IAdditionalInfoRepository" />
    public class AdditionalInfoRepository : BaseRepository<AdditionalInfo>, IAdditionalInfoRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalInfoRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AdditionalInfoRepository(IConfiguration configuration) : base("additional_info", configuration)
        {
            this.configuration = configuration;
        }
    }
}

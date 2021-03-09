using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class FcmInfoRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.FcmInfo}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IFcmInfoRepository" />
    public class FcmInfoRepository : BaseRepository<FcmInfo>, IFcmInfoRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FcmInfoRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FcmInfoRepository(IConfiguration configuration) : base("fcm_info", configuration)
        {
            this.configuration = configuration;
        }
    }
}

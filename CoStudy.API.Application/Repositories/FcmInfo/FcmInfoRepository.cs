using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class FcmInfoRepository : BaseRepository<FcmInfo>, IFcmInfoRepository
    {
        IConfiguration configuration;
        public FcmInfoRepository(IConfiguration configuration) : base("fcm_info", configuration)
        {
            this.configuration = configuration;
        }
    }
}

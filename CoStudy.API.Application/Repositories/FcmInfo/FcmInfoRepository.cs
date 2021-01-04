using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class FcmInfoRepository : BaseRepository<FcmInfo>, IFcmInfoRepository
    {
        public FcmInfoRepository() : base("fcm_info")
        {

        }
    }
}

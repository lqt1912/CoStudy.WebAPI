using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class AdditionalInfoRepository : BaseRepository<AdditionalInfo>, IAdditionalInfoRepository
    {
        public AdditionalInfoRepository() : base("additional_info")
        {

        }
    }
}

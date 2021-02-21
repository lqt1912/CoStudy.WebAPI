using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class AdditionalInfoRepository : BaseRepository<AdditionalInfo>, IAdditionalInfoRepository
    {
        IConfiguration configuration;
        public AdditionalInfoRepository(IConfiguration configuration) : base("additional_info", configuration)
        {
            this.configuration = configuration;
        }
    }
}

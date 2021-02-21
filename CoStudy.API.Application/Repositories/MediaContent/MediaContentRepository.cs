using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class MediaContentRepository : BaseRepository<MediaContent>, IMediaContentRepository
    {
        IConfiguration configuration;
        public MediaContentRepository(IConfiguration configuration) : base("media_content", configuration)
        {
            this.configuration = configuration;
        }
    }
}

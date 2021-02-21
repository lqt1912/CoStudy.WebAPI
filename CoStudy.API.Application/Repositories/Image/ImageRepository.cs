using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        IConfiguration configuration;
        public ImageRepository(IConfiguration configuration) : base("image", configuration)
        {
            this.configuration = configuration;
        }
    }
}

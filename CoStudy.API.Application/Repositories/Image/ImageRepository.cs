using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class ImageRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Image}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IImageRepository" />
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ImageRepository(IConfiguration configuration) : base("image", configuration)
        {
            this.configuration = configuration;
        }
    }
}

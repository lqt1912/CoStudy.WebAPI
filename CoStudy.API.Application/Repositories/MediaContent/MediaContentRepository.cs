using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MediaContentRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MediaContent}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMediaContentRepository" />
    public class MediaContentRepository : BaseRepository<MediaContent>, IMediaContentRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaContentRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MediaContentRepository(IConfiguration configuration) : base("media_content", configuration)
        {
            this.configuration = configuration;
        }
    }
}

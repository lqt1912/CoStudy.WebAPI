using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class PostRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Post}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IPostRepository" />
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public PostRepository(IConfiguration configuration) : base("post", configuration)
        {
            this.configuration = configuration;
        }
    }
}

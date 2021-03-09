using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class CommentRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Comment}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.ICommentRepository" />
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CommentRepository(IConfiguration configuration) : base("comment", configuration)
        {
            this.configuration = configuration;
        }
    }
}

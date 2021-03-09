using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class UpVoteRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.UpVote}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IUpVoteRepository" />
    public class UpVoteRepository : BaseRepository<UpVote>, IUpVoteRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="UpVoteRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public UpVoteRepository(IConfiguration configuration) : base("upvote", configuration)
        {
            this.configuration = configuration;
        }
    }

    /// <summary>
    /// Class DownVoteRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.DownVote}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IDownVoteRepository" />
    public class DownVoteRepository : BaseRepository<DownVote>, IDownVoteRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="DownVoteRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DownVoteRepository(IConfiguration configuration) : base("downvote", configuration)
        {
            this.configuration = configuration;
        }
    }
}

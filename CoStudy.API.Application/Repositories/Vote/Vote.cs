using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class UpVoteRepository : BaseRepository<UpVote>, IUpVoteRepository
    {
        IConfiguration configuration;
        public UpVoteRepository(IConfiguration configuration) : base("upvote", configuration)
        {
            this.configuration = configuration;
        }
    }

    public class DownVoteRepository : BaseRepository<DownVote>, IDownVoteRepository
    {
        IConfiguration configuration;
        public DownVoteRepository(IConfiguration configuration) : base("downvote", configuration)
        {
            this.configuration = configuration;
        }
    }
}

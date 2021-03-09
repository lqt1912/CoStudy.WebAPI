using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Interface IUpVoteRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.UpVote}" />
    public interface IUpVoteRepository : IBaseRepository<UpVote>
    {
    }

    /// <summary>
    /// Interface IDownVoteRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.DownVote}" />
    public interface IDownVoteRepository : IBaseRepository<DownVote>
    {

    }
}

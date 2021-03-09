using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// interface ICommentRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.Comment}" />
    public interface ICommentRepository : IBaseRepository<Comment>
    {

    }
}

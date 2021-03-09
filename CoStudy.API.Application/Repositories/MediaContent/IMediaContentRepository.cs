using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Interface IMediaContentRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.MediaContent}" />
    public interface IMediaContentRepository : IBaseRepository<MediaContent>
    {

    }
}

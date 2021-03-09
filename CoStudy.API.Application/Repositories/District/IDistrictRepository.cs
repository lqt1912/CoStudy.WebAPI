using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// interface IDistrictRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.District}" />
    public interface IDistrictRepository:IBaseRepository<District>
    {
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Interface IWardRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.Ward}" />
    public interface IWardRepository:IBaseRepository<Ward>
    {
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;

namespace CoStudy.API.Application.Repositories
{
    public class FieldRepository : BaseRepository<Field>, IFieldRepository
    {
        public FieldRepository() : base("field")
        {

        }
    }
}

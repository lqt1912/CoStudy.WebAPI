using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class FieldRepository : BaseRepository<Field>, IFieldRepository
    {
        IConfiguration configuration;
        public FieldRepository(IConfiguration configuration) : base("field", configuration)
        {
            this.configuration = configuration;
        }
    }
}

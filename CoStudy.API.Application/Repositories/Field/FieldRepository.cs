using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class FieldRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Field}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IFieldRepository" />
    public class FieldRepository : BaseRepository<Field>, IFieldRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FieldRepository(IConfiguration configuration) : base("field", configuration)
        {
            this.configuration = configuration;
        }
    }
}

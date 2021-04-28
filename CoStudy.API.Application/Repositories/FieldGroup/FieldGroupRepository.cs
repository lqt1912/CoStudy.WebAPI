using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class FieldGroupRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.FieldGroup}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IFieldGroupRepository" />
    public class FieldGroupRepository : BaseRepository<FieldGroup>, IFieldGroupRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FieldGroupRepository(IConfiguration configuration) : base("field_group", configuration)
        {
            this.configuration = configuration;
        }
    }
}

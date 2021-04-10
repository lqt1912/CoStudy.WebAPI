using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class MessageMultiMediaRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.MessageMultiMedia}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IMessageMultiMediaRepository" />
    public class MessageMultiMediaRepository : BaseRepository<MessageMultiMedia>, IMessageMultiMediaRepository
    {

        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageMultiMediaRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessageMultiMediaRepository(IConfiguration configuration) : base("message_multimedia", configuration)
        {
            this.configuration = configuration;
        }
    }
}

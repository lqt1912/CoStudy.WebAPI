using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class NotificationObjectRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.NotificationObject}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.INotificationObjectRepository" />
    public class NotificationObjectRepository : BaseRepository<NotificationObject>, INotificationObjectRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationObjectRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public NotificationObjectRepository(IConfiguration configuration) : base("notification_object", configuration)
        {
            this.configuration = configuration;
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Interface NotificationTypeRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.NotificationType}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.INotificationTypeRepository" />
    public class NotificationTypeRepository : BaseRepository<NotificationType>, INotificationTypeRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public NotificationTypeRepository(IConfiguration configuration) : base("notification_type", configuration)
        {
            this.configuration = configuration;
        }
    }
}

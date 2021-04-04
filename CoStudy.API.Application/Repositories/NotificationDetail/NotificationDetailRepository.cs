using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class NotificationDetailRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.NotificationDetail}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.INotificationDetailRepository" />
    public class NotificationDetailRepository :BaseRepository<NotificationDetail>,INotificationDetailRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDetailRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public NotificationDetailRepository(IConfiguration configuration ) :base("notification_detail", configuration)
        {
            this.configuration = configuration;
        }
    }
}

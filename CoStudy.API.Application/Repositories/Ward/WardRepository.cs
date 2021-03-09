using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class WardRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Ward}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IWardRepository" />
    public class WardRepository:BaseRepository<Ward>, IWardRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WardRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public WardRepository(IConfiguration configuration) : base("ward", configuration)
        {
            this.configuration = configuration;
        }
    }
}

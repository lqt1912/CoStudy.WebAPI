using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class LevelRepository 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Level}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.ILevelRepository" />
    public class LevelRepository : BaseRepository<Level>, ILevelRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LevelRepository(IConfiguration configuration) : base("level", configuration)
        {
            this.configuration = configuration;
        }
    }
}

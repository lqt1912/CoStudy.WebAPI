using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// class DistrictRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.District}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IDistrictRepository" />
    public class DistrictRepository:BaseRepository<District>, IDistrictRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DistrictRepository(IConfiguration configuration) : base("district", configuration)
        {
            this.configuration = configuration;
        }
    }
}

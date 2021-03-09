using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class ProvinceRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Province}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IProvinceRepository" />
    public class ProvinceRepository:BaseRepository<Province>, IProvinceRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ProvinceRepository(IConfiguration configuration) : base("province", configuration)
        {
            this.configuration = configuration;
        }
    }
}

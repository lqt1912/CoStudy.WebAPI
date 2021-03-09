using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// The Report  Repository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Report}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IReportRepository" />
    public class ReportRepository :BaseRepository<Report>, IReportRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ReportRepository(IConfiguration configuration):base("report", configuration)
        {
            this.configuration = configuration;
        }
    }
}

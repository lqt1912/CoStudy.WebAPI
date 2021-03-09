using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{

    /// <summary>
    /// The Report Repository 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.ReportReason}" />
    public class ReportReasonRepository:BaseRepository<ReportReason>, IReportReasonRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportReasonRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ReportReasonRepository(IConfiguration configuration):base("report_reason",configuration)
        {
            this.configuration = configuration;
        }
    }
}

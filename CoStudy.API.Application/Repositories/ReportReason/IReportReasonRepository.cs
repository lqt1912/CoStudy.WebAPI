using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Application.Repositories
{

    /// <summary>
    /// The Report Reason interface
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.ReportReason}" />
    public interface IReportReasonRepository : IBaseRepository<ReportReason>
    {
    }
}

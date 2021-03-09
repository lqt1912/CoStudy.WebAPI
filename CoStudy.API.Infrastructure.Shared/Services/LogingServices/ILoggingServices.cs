using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface ILoggingServices
    {
        TableResultJson<LoggingViewModel> GetPaged(TableRequest request);

        Task<IEnumerable<int>> CountResultCode();
    }
}

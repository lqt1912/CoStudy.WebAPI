using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface ILoggingServices
    {
             TableResultJson<LoggingViewModel> GetPaged(TableRequest request);


            Task<IEnumerable<int>> CountResultCode();

             Task<string> Delete(DeleteLoggingRequest request);

        Task<LoggingViewModel> GetById(string id);

    }
}

using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Interface ILoggingServices
    /// </summary>
    public interface ILoggingServices
    {
        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        TableResultJson<LoggingViewModel> GetPaged(TableRequest request);


        /// <summary>
        /// Counts the result code.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<int>> CountResultCode();

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<string> Delete(DeleteLoggingRequest request);

        Task<LoggingViewModel> GetById(string id);

    }
}

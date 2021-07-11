using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface IViolenceWordService
    {
        Task<ViolenceWordViewModel> Add(string value);
        IEnumerable<ViolenceWordViewModel> GetAll(BaseGetAllRequest request);
        Task<string> Delete(string id);
        TableResultJson<ViolenceWordViewModel> GetViolenceWordPaged(TableRequest request);
    }
}

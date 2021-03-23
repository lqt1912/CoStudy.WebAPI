using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public interface INofticationService
    {
        Task<NotificationViewModel> AddNoftication(AddNofticationRequest request);
        Task<IEnumerable<NotificationViewModel>> GetCurrentUserNoftication(BaseGetAllRequest request);

        Task<string> DeleteNotification(string id);

        Task<string> MarkAsRead(string id);
    }
}

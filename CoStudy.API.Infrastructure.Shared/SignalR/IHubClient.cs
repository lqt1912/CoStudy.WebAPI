using CoStudy.API.Domain.Entities.Application;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.SignalR
{
    public interface IHubClient
    {
        Task BroadcastMessage(Message msg);
    }
}

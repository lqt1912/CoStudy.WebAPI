using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR
{
    public interface IHubClient<T> where T : class
    {
        Task SendNofti(T msg);
    }
}

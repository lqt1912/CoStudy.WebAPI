using System.Threading;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.BackgroundTask.WorkerService
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}

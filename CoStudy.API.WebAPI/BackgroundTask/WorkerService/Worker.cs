using CoStudy.API.Infrastructure.Shared.Hangfire;
using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.BackgroundTask.WorkerService
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> logger;

        private readonly IHangfireService hangfireService;

        public Worker(ILogger<Worker> logger, IHangfireService hangfireService)
        {
            this.logger = logger;
            this.hangfireService = hangfireService;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //await hangfireService.RemoveViolencePost();
                await Task.Delay(1000);
            }
        }
    }
}

using CoStudy.API.WebAPI.BackgroundTask.WorkerService;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.BackgroundTask
{
    public class DerivedBackgroundPrinter : BackgroundService
    {
        private readonly IWorker worker;

        public DerivedBackgroundPrinter(IWorker worker)
        {
            this.worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await worker.DoWork(stoppingToken);
        }
    }
}

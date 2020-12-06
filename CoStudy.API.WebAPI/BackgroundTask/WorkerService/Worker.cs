using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.BackgroundTask.WorkerService
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> logger;

        IUserService userService;
        IPostService postService;

        public Worker(ILogger<Worker> logger, IUserService userService, IPostService postService)
        {
            this.logger = logger;
            this.userService = userService;
            this.postService = postService;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await userService.SyncPost();
                await postService.SyncComment();
                await postService.SyncReply();

                logger.LogInformation($"Syncing");

                await Task.Delay(1000);
            }
        }
    }
}

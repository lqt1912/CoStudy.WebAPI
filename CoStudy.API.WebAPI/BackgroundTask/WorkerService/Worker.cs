using CoStudy.API.Infrastructure.Shared.Services.PostServices;
using CoStudy.API.Infrastructure.Shared.Services.UserServices;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.BackgroundTask.WorkerService
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CoStudy.API.WebAPI.BackgroundTask.WorkerService.IWorker" />
    public class Worker : IWorker
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<Worker> logger;

        /// <summary>
        /// The user service
        /// </summary>
        IUserService userService;
        /// <summary>
        /// The post service
        /// </summary>
        IPostService postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="postService">The post service.</param>
        public Worker(ILogger<Worker> logger, IUserService userService, IPostService postService)
        {
            this.logger = logger;
            this.userService = userService;
            this.postService = postService;
        }

        /// <summary>
        /// Does the work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation($"Syncing");

                await Task.Delay(1000);
            }
        }
    }
}

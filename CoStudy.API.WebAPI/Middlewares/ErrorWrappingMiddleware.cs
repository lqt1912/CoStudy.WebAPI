using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Middlewares
{
    /// <summary>
    /// Error wrapping middleware
    /// </summary>
    public class ErrorWrappingMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ErrorWrappingMiddleware> _logger;

        /// <summary>
        /// The logging repository
        /// </summary>
        private readonly ILoggingRepository loggingRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWrappingMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="loggingRepository">The logging repository.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger, ILoggingRepository loggingRepository)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.loggingRepository = loggingRepository;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            bool success = true;
            string message = string.Empty;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                context.Response.OnStarting(() =>
                {
                    // Stop the timer information and calculate the time   
                    sw.Stop();
                    var responseTimeForCompleteRequest = sw.ElapsedMilliseconds;
                    // Add the Response time information in the Response headers.   
                    context.Response.Headers["X-Response-Time-ms"] = responseTimeForCompleteRequest.ToString();
                    return Task.CompletedTask;
                });


                await _next.Invoke(context);
                //  sw.Stop();

                int? statusCode = context.Response?.StatusCode;

                LogEventLevel level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                if (level == LogEventLevel.Information && !context.Request.Path.ToString().ToLower().Contains("logging"))
                {
                    Logging logging = new Logging();
                    logging.RequestMethod = context.Request.Method;
                    logging.Location = $"{context.Request.Scheme}://{context.Request.Host}";
                    logging.RequestPath = context.Request.Path.ToString();
                    logging.StatusCode = statusCode.Value;
                    logging.TimeElapsed = sw.Elapsed.TotalMilliseconds;
                    logging.Message = "Request success";
                    logging.Ip = context.Connection.RemoteIpAddress.ToString();
                    logging.CreatedDate = DateTime.Now;
                    await loggingRepository.AddAsync(logging);
                }
            }
            catch (Exception ex)
            {
                ExceptionMessageModel messageDetail = ReadException(ex.Message);
                _logger.LogError(10000, ex, ex.Message);
                message = messageDetail.Message;

                context.Response.StatusCode = 400;

                success = false;

                Logging logging = new Logging();
                logging.RequestMethod = context.Request.Method;
                logging.Location = $"{context.Request.Scheme}://{context.Request.Host}";
                logging.RequestPath = context.Request.Path.ToString();
                logging.StatusCode = 400;
                logging.TimeElapsed = sw.Elapsed.TotalMilliseconds;
                logging.Message = message;
                logging.CreatedDate = DateTime.Now;
                logging.Ip = context.Connection.RemoteIpAddress.ToString();
                await loggingRepository.AddAsync(logging);
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                ApiResponse response = new ApiResponse(success, context.Response.StatusCode, message);

                string json = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(json);
            }
        }

        /// <summary>
        /// Reads the exception.
        /// </summary>
        /// <param name="messageException">The message exception.</param>
        /// <returns></returns>
        private ExceptionMessageModel ReadException(string messageException)
        {
            ExceptionMessageModel exceptionMessageModel = new ExceptionMessageModel();
            try
            {
                return JsonConvert.DeserializeObject<ExceptionMessageModel>(messageException);
            }
            catch (Exception)
            {

            }
            exceptionMessageModel.Message = messageException;
            return exceptionMessageModel;
        }


    }
}

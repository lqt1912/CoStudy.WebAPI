using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Middlewares
{
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
        /// Initializes a new instance of the <see cref="ErrorWrappingMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            bool success = true;
            var message = string.Empty;
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var messageDetail = ReadException(ex.Message);
                _logger.LogError(10000, ex, ex.Message);
                message = messageDetail.Message;

                context.Response.StatusCode = 400;

                success = false;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new ApiResponse(success, context.Response.StatusCode, message);

                var json = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(json);
            }
        }

        private ExceptionMessageModel ReadException(string messageException)
        {
            ExceptionMessageModel exceptionMessageModel = new ExceptionMessageModel();
            try
            {
                return JsonConvert.DeserializeObject<ExceptionMessageModel>(messageException);
            }
            catch (Exception ex)
            {

            }
            exceptionMessageModel.Message = messageException;
            return exceptionMessageModel;
        }
    }
}

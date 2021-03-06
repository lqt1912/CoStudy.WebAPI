﻿using CoStudy.API.Application.Repositories;
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
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorWrappingMiddleware> _logger;

        private readonly ILoggingRepository loggingRepository;

        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger, ILoggingRepository loggingRepository)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.loggingRepository = loggingRepository;
        }

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

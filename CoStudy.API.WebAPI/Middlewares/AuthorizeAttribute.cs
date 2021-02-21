using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.WebAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;


        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Account account = (Account)context.HttpContext.Items["Account"];

            ILoggingRepository loggingRepository = (ILoggingRepository)context.HttpContext.RequestServices.GetService(typeof(ILoggingRepository));

            if (account == null || (_roles.Any() && !_roles.Contains(account.Role)))
            {
                ApiResponse response = new ApiResponse(false, StatusCodes.Status401Unauthorized, "Unauthorized");
                // not logged in or role not authorized
                context.Result = new JsonResult(response);

                Logging logging = new Logging();
                logging.Location = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
                logging.RequestMethod = context.HttpContext.Request.Method;
                logging.RequestPath = context.HttpContext.Request.Path.ToString();
                logging.StatusCode = StatusCodes.Status401Unauthorized;
                logging.Message = "Unauthorized";
                logging.Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                logging.CreatedDate = DateTime.Now;
                loggingRepository.Add(logging);
            }
        }
    }
}

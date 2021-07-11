using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.WebAPI.Middlewares
{
    public class ApiNotFoundResponse : ApiResponse
    {
        public string  NotFoundMessage { get; set; }

        public ApiNotFoundResponse(string _message) : base(false, 404)
        {
            NotFoundMessage = _message;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoStudy.API.WebAPI.Middlewares
{
    public class ApiNotFoundResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public ApiNotFoundResponse(ModelStateDictionary modelState) : base(false, 404)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }
    }
}

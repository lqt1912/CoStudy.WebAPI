using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoStudy.API.WebAPI.Middlewares
{
    /// <summary>
    /// Api notfound response
    /// </summary>
    /// <seealso cref="CoStudy.API.WebAPI.Middlewares.ApiResponse" />
    public class ApiNotFoundResponse : ApiResponse
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiNotFoundResponse"/> class.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <exception cref="ArgumentException">ModelState must be invalid - modelState</exception>
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

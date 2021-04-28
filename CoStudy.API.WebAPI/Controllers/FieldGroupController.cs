using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Services;
using CoStudy.API.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    /// <summary>
    /// Class FieldGroupController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class FieldGroupController : ControllerBase
    {
        /// <summary>
        /// The field service
        /// </summary>
        IFieldServices fieldService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupController"/> class.
        /// </summary>
        /// <param name="fieldService">The field service.</param>
        public FieldGroupController(IFieldServices fieldService)
        {
            this.fieldService = fieldService;
        }

        /// <summary>
        /// Adds the field group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddFieldGroup(FieldGroup request)
        {
            var data = await fieldService.AddFieldGroup(request);
            return Ok(new ApiOkResponse(data));
        }


    }
}

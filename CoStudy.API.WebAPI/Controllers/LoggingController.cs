﻿using CoStudy.API.Infrastructure.Shared.Paging;
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
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        ILoggingServices loggingServices;

        public LoggingController(ILoggingServices loggingServices)
        {
            this.loggingServices = loggingServices;
        }

        [HttpPost, Route("paged")]
        public IActionResult GetPaged(TableRequest request)
        {
            var data = loggingServices.GetPaged(request);
            return Ok(data);
        }

        [HttpGet, Route("count")]
        public async Task<IActionResult> CountStatusCode()
        { 
            var data =await loggingServices.CountResultCode();
            return Ok(new ApiOkResponse(data));
        }
    }
}

using CoStudy.API.Infrastructure.Shared.Hangfire;
using Hangfire;
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
    public class HangfireController : ControllerBase
    {
        IHangfireService hangfireService;

        public HangfireController(IHangfireService hangfireService)
        {
            this.hangfireService = hangfireService;
        }

    }
}

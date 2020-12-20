using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.WebAPI.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private IHubContext<SignalRHub<Message>, IHubClient<Message>> _signalrHub;

        public SignalRController(IHubContext<SignalRHub<Message>, IHubClient<Message>> signalrHub)
        {
            _signalrHub = signalrHub;
        }

        [HttpPost] 
        public IActionResult Post(Message msg)
        {
            _signalrHub.Clients.All.SendNofti(msg);
            return Ok("send!");
        }
    }
}

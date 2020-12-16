﻿using CoStudy.API.Domain.Entities.Application;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.SignalR
{
    public class SignalRHub : Hub<IHubClient>
    {
        public async Task BroadcastMessage(Message msg)
        {
            await Clients.All.BroadcastMessage(msg);
        }
    }
}

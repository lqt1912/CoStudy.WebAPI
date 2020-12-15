using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.SignalR
{
        public interface IHubClient
        {
            Task BroadcastMessage(Message msg);
        }
}

using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR.DI.Message
{
    public interface IMessageHub
    {
        Task SendGlobal(AddMessageResponse addMessageResponse);
    }
}

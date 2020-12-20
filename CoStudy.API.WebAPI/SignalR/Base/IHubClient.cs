using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR
{
    public interface IHubClient<T> where T : class
    {
        Task SendNofti(T msg);
    }
}

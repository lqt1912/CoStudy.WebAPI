using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public interface IBaseHub<T> where T:class
    {
        Task BroadCast(T message);

    }
}

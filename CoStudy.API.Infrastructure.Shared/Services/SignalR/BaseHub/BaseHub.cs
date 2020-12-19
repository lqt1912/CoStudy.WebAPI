using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
  
   //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseHub<T> : Hub<IBaseHub<T>> where T : class
    {
        public IClientConnectionsRepository clientConnectionsRepository { get; }

        public IClientGroupRepository clientGroupRepository { get; }
        public IHttpContextAccessor httpContextAccessor { get; }
        public IUserRepository userRepository { get; }

        public BaseHub(IClientConnectionsRepository clientConnectionsRepository, IClientGroupRepository clientGroupRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.clientConnectionsRepository = clientConnectionsRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public override  Task OnConnectedAsync()
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var clientConnection =  clientConnectionsRepository.GetById(currentUser.Id);
            clientConnection.ClientConnection.Add(Context.ConnectionId);
            clientConnectionsRepository.Update(clientConnection, clientConnection.Id);

            return base.OnConnectedAsync();
        }

        public override  Task OnDisconnectedAsync(Exception exception)
        {

            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var clientConnection =  clientConnectionsRepository.GetById(currentUser.Id);
            clientConnection.ClientConnection.Remove(Context.ConnectionId);
             clientConnectionsRepository.Update(clientConnection, clientConnection.Id);

            return  base.OnDisconnectedAsync(exception);
        }

        public async Task BroadCast(T message)
        {
            await Clients.All.BroadCast(message);
        }
    }
}

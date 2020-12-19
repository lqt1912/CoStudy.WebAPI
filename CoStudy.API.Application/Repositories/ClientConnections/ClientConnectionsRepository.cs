using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class ClientConnectionsRepository :BaseRepository<ClientConnections>, IClientConnectionsRepository
    {
        public ClientConnectionsRepository():base("client_connections")
        {

        }
    }
}

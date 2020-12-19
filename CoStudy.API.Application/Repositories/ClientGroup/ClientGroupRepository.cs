using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class ClientGroupRepository :BaseRepository<ClientGroup>, IClientGroupRepository
    {
        public ClientGroupRepository():base("client_group")
        {

        }
    }
}

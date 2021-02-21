using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class WardRepository:BaseRepository<Ward>, IWardRepository
    {
        IConfiguration configuration;
        public WardRepository(IConfiguration configuration) : base("ward", configuration)
        {
            this.configuration = configuration;
        }
    }
}

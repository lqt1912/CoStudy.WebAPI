using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class ObjectLevelRepository :BaseRepository<ObjectLevel>, IObjectLevelRepository
    {
        IConfiguration configuration;
        public ObjectLevelRepository(IConfiguration configuration) : base("object_level", configuration)
        {
            this.configuration = configuration;
        }
    }
}

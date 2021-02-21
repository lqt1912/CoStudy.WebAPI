using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class LevelRepository : BaseRepository<Level>, ILevelRepository
    {
        IConfiguration configuration;
        public LevelRepository(IConfiguration configuration) : base("level", configuration)
        {
            this.configuration = configuration;
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class DistrictRepository:BaseRepository<District>, IDistrictRepository
    {
        IConfiguration configuration;
        public DistrictRepository(IConfiguration configuration) : base("district", configuration)
        {
            this.configuration = configuration;
        }
    }
}

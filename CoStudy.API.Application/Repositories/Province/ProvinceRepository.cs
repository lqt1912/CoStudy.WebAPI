using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class ProvinceRepository:BaseRepository<Province>, IProvinceRepository
    {
        IConfiguration configuration;
        public ProvinceRepository(IConfiguration configuration) : base("province", configuration)
        {
            this.configuration = configuration;
        }
    }
}

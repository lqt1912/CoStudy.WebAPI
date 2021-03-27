using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository
{
    public class ExternalLoginRepository :BaseRepository<ExternalLogin>, IExternalLoginRepository
    {
        IConfiguration configuration;
        public ExternalLoginRepository(IConfiguration configuration) : base("external_login", configuration)
        {
            this.configuration = configuration;
        }
    }
}

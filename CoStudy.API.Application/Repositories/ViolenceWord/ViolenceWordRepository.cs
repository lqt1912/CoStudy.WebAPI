using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class ViolenceWordRepository : BaseRepository<ViolenceWord>, IViolenceWordRepository
    {
        private readonly IConfiguration configuration;

        public ViolenceWordRepository(IConfiguration configuration) :base("violence_word", configuration)
        {
            this.configuration = configuration;
        }
    }
}

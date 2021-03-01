using CoStudy.API.Application.Repositoriest;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        IConfiguration configuration;
        public DocumentRepository(IConfiguration configuration) : base("document", configuration)
        {
            this.configuration = configuration;
        }
    }
}

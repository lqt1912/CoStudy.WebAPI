using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// The Document Repository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.Document}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IDocumentRepository" />
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DocumentRepository(IConfiguration configuration) : base("document", configuration)
        {
            this.configuration = configuration;
        }
    }
}

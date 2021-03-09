using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// interface IDocumentRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.IBaseRepository{CoStudy.API.Domain.Entities.Application.Document}" />
    public interface IDocumentRepository :IBaseRepository<Document>
    {
    }
}

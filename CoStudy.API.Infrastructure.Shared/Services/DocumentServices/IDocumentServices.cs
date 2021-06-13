using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
       public interface IDocumentServices
    {
             Task<Document> Add(Document entity);

             IEnumerable<Document> GetAll(BaseGetAllRequest request);

             Task<Document> GetById(string id);

             Task<string> Delete(string id);
    }
}

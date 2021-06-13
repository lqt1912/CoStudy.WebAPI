using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
        public class DocumentServices : IDocumentServices
    {

           IConfiguration configuration;

           IDocumentRepository documentRepository;

           IHttpContextAccessor httpContextAccessor;

              public DocumentServices(IConfiguration configuration, IDocumentRepository documentRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.documentRepository = documentRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

             public async Task<Document> Add(Document entity)
        {
            var document = new Document()
            {
                CreatedDate = DateTime.Now,
                Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Length = entity.Length,
                LocalName = entity.LocalName,
                ObjectId = entity.ObjectId,
                ServerName = entity.ServerName,
                UpdateDate = DateTime.Now,
                UploadBy = entity.UploadBy,
            };

            await documentRepository.AddAsync(document);
            return document;
        }

             public IEnumerable<Document> GetAll(BaseGetAllRequest request)
        {

            var data = documentRepository.GetAll();

            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return data;
        }

             public async Task<Document> GetById(string id)
        {
            var data = await documentRepository.GetByIdAsync(ObjectId.Parse(id));
            return data;
        }

              public async Task<string> Delete(string id)
        {
            try
            {
                await documentRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch (Exception)
            {
                throw new Exception("Xóa không thành công. ");
            }

        }

    }
}

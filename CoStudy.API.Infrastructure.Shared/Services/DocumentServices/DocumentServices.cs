using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Document service class.
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IDocumentServices" />
    public class DocumentServices : IDocumentServices
    {

        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// The document repository
        /// </summary>
        IDocumentRepository documentRepository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentServices" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="documentRepository">The document repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public DocumentServices(IConfiguration configuration, IDocumentRepository documentRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.documentRepository = documentRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Document> GetAll(BaseGetAllRequest request)
        {

            var data = documentRepository.GetAll();

            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }

            return data;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Document> GetById(string id)
        {
            var data = await documentRepository.GetByIdAsync(ObjectId.Parse(id));
            return data;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Xóa không thành công.</exception>
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

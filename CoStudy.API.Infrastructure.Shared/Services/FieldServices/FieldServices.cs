using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// The Field Service. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.IFieldServices" />
    public class FieldServices:IFieldServices
    {
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldServices"/> class.
        /// </summary>
        /// <param name="fieldRepository">The field repository.</param>
        public FieldServices(IFieldRepository fieldRepository)
        {
            this.fieldRepository = fieldRepository;
        }

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<Field> AddField(Field entity)
        {
            var data = new Field()
            {
                Value = entity.Value
            };

            await fieldRepository.AddAsync(entity);
            return data;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IEnumerable<Field> GetAll(BaseGetAllRequest request)
        {
            var data = fieldRepository.GetAll();
            if(request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return data;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Xóa thất bại.</exception>
        public async Task<string> Delete(String id)
        {
            try
            {
                await fieldRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch(Exception)
            {
                throw new Exception("Xóa thất bại.");
            }
        }
    }
}

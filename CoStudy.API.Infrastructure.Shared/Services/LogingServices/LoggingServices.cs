using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.LoggingRequest;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    /// <summary>
    /// Class LoggingServices
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Services.ILoggingServices" />
    public class LoggingServices : ILoggingServices
    {
        /// <summary>
        /// The logging repository
        /// </summary>
        ILoggingRepository loggingRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingServices"/> class.
        /// </summary>
        /// <param name="loggingRepository">The logging repository.</param>
        /// <param name="mapper">The mapper.</param>
        public LoggingServices(ILoggingRepository loggingRepository, IMapper mapper)
        {
            this.loggingRepository = loggingRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Counts the result code.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<int>> CountResultCode()
        {
            var result = new List<int>();
            result.Add(0); 
            result.Add(0);
            result.Add(0);
            result.Add(0);

            var builder400 = Builders<Logging>.Filter.Eq("StatusCode", 400) ;
            var builder401 = Builders<Logging>.Filter.Eq("StatusCode", 401);
            var builder200 = Builders<Logging>.Filter.Eq("StatusCode", 200);

            result[0] = (await loggingRepository.FindListAsync(builder200)).Count();
            result[1] = (await loggingRepository.FindListAsync(builder400)).Count();
            result[2] = (await loggingRepository.FindListAsync(builder401)).Count();

            result[3] = (int)((await loggingRepository.CountAsync()) - result[0] - result[1] - result[2]);

            return result;
        }

        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public TableResultJson<LoggingViewModel> GetPaged(TableRequest request)
        {
            var dataSource = loggingRepository.GetAll().OrderByDescending(x => x.CreatedDate.Value).AsEnumerable();

            if (request.columns[0].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[0].search.value))
                    dataSource = dataSource.Where(x => x.RequestMethod.Contains(request.columns[0].search.value));
            }

            if (request.columns[1].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                    dataSource = dataSource.Where(x => x.Location.Contains(request.columns[1].search.value));
            }

            if (request.columns[3].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                    dataSource = dataSource.Where(x => x.StatusCode.ToString().Contains(request.columns[3].search.value));
            }
            var response = new TableResultJson<LoggingViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSource.Count();

            dataSource = dataSource.Skip(request.start).Take(request.length);
            response.data = mapper.Map<List<LoggingViewModel>>(dataSource.ToList());
            foreach (var item in response.data)
            {
                item.Index = response.data.IndexOf(item)+ request.start +1;
            }
            return response;
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<string> Delete(DeleteLoggingRequest request)
        {
            if (request.Ids == null)
                request.Ids = new List<string>();

            int count = 0;
            foreach (var id in request.Ids)
            {
                var existLogging = await loggingRepository.GetByIdAsync(ObjectId.Parse(id));
                if(existLogging!=null)
                {
                    await loggingRepository.DeleteAsync(existLogging.Id);
                    count++;
                }
            }
            return $"Đã xóa {count} đối tượng. ";

        }
    }
}

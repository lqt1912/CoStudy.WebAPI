using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class LoggingServices : ILoggingServices
    {
        ILoggingRepository loggingRepository;

        IMapper mapper;

        public LoggingServices(ILoggingRepository loggingRepository, IMapper mapper)
        {
            this.loggingRepository = loggingRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<int>> CountResultCode()
        {
            var result = new List<int> {0, 0, 0, 0};

            var builder400 = Builders<Logging>.Filter.Eq("StatusCode", 400);
            var builder401 = Builders<Logging>.Filter.Eq("StatusCode", 401);
            var builder200 = Builders<Logging>.Filter.Eq("StatusCode", 200);

            result[0] = (await loggingRepository.FindListAsync(builder200)).Count();
            result[1] = (await loggingRepository.FindListAsync(builder400)).Count();
            result[2] = (await loggingRepository.FindListAsync(builder401)).Count();

            result[3] = (int)((await loggingRepository.CountAsync()) - result[0] - result[1] - result[2]);

            return result;
        }

        public TableResultJson<LoggingViewModel> GetPaged(TableRequest request)
        {
            var dataSource = loggingRepository.GetAll().OrderByDescending(x => x.CreatedDate.Value).AsEnumerable();

            if (request.columns[0].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[0].search.value))
                {
                    dataSource = dataSource.Where(x => x.RequestMethod.Contains(request.columns[0].search.value));
                }
            }

            if (request.columns[1].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[1].search.value))
                {
                    dataSource = dataSource.Where(x => x.Location.Contains(request.columns[1].search.value));
                }
            }

            if (request.columns[3].search != null)
            {
                if (!string.IsNullOrEmpty(request.columns[3].search.value))
                {
                    dataSource = dataSource.Where(x => x.StatusCode.ToString().Contains(request.columns[3].search.value));
                }
            }
            var response = new TableResultJson<LoggingViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSource.Count();

            dataSource = dataSource.Skip(request.start).Take(request.length);
            response.data = mapper.Map<List<LoggingViewModel>>(dataSource.ToList());
            foreach (LoggingViewModel item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }
            return response;
        }

        public async Task<string> Delete(DeleteLoggingRequest request)
        {
            if (request.Ids == null)
            {
                request.Ids = new List<string>();
            }

            int count = 0;
            foreach (string id in request.Ids)
            {
                Logging existLogging = await loggingRepository.GetByIdAsync(ObjectId.Parse(id));
                if (existLogging != null)
                {
                    await loggingRepository.DeleteAsync(existLogging.Id);
                    count++;
                }
            }
            return $"Đã xóa {count} đối tượng. ";

        }

        public async Task<LoggingViewModel> GetById(string id)
        {
            Logging a = await loggingRepository.GetByIdAsync(ObjectId.Parse(id));
            return mapper.Map<LoggingViewModel>(a);
        }
    }

}
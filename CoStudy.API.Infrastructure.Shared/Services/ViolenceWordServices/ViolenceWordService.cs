using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class ViolenceWordService: IViolenceWordService
    {
        private readonly IViolenceWordRepository violenceWordRepository;
        private readonly IMapper mapper;

        public ViolenceWordService(IViolenceWordRepository violenceWordRepository, IMapper mapper)
        {
            this.violenceWordRepository = violenceWordRepository;
            this.mapper = mapper;
        }

        public async Task<ViolenceWordViewModel> Add(string value)
        {
            var entity = new ViolenceWord()
            {
                Value = value
            };
            await violenceWordRepository.AddAsync(entity);
            return mapper.Map<ViolenceWordViewModel>(entity);
        }

        public async Task<string> Delete(string id)
        {
            try
            {
                await violenceWordRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công. ";
            }
            catch (Exception)
            {
                return "Xóa thất bại. ";
            }
        }

        public IEnumerable<ViolenceWordViewModel> GetAll(BaseGetAllRequest request)
        {
            var data = violenceWordRepository.GetAll();
            if(request.Skip.HasValue && request.Count.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return mapper.Map<IEnumerable<ViolenceWordViewModel>>(data);
        }

        public TableResultJson<ViolenceWordViewModel> GetViolenceWordPaged(TableRequest request)
        {
            var dataSource = violenceWordRepository.GetAll();
            var dataSourceViewModel = mapper.Map<IEnumerable<ViolenceWordViewModel>>(dataSource);

            var response = new TableResultJson<ViolenceWordViewModel>();
            response.draw = request.draw;
            response.recordsFiltered = dataSourceViewModel.Count();
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();

            foreach (var item in response.data)
            {
                item.Index = response.data.IndexOf(item) + request.start + 1;
            }
            return response;
        }
    }
}

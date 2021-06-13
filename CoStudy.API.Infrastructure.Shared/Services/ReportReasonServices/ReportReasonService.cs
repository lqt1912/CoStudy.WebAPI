using AutoMapper;
using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services
{
        public class ReportReasonService : IReportReasonService
    {
           IReportReasonRepository reportReasonRepository;

           IHttpContextAccessor httpContextAccessor;

           IUserRepository userRepository;

           IMapper mapper;

            public ReportReasonService(IReportReasonRepository reportReasonRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.reportReasonRepository = reportReasonRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

             public async Task<ReportReasonViewModel> Add(ReportReason entity)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);
            var data = new ReportReason()
            {
                Detail = entity.Detail,
                CreatedBy = currentUser.OId
            };
            await reportReasonRepository.AddAsync(data);
            return mapper.Map<ReportReasonViewModel>(data);
        }

             public IEnumerable<ReportReasonViewModel> GetAll(BaseGetAllRequest request)
        {
            var data = reportReasonRepository.GetAll();
            if (request.Count.HasValue && request.Skip.HasValue)
            {
                data = data.Skip(request.Skip.Value).Take(request.Count.Value);
            }
            return mapper.Map<IEnumerable<ReportReasonViewModel>>(data);
        }


              public async Task<string> Delete(string id)
        {
            try
            {
                await reportReasonRepository.DeleteAsync(ObjectId.Parse(id));
                return "Xóa thành công";
            }
            catch (Exception)
            {
                throw new Exception("Xóa thất bại");
            }
        }
    }
}

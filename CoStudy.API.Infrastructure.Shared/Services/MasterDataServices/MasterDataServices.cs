using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.Paging;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoStudy.API.Domain.Entities.Application;
using MongoDB.Driver.Core.Authentication;

namespace CoStudy.API.Infrastructure.Shared.Services
{
    public class MasterDataServices : IMasterDataServices
    {
        IFieldRepository fieldRepository;
        ILevelRepository levelRepository;
        IReportReasonRepository reportReasonRepository;
        INotificationTypeRepository notificationTypeRepository;
        private IFieldGroupRepository fieldGroupRepository;
        IMapper mapper;

        public MasterDataServices(IFieldRepository fieldRepository, ILevelRepository levelRepository, IReportReasonRepository reportReasonRepository, INotificationTypeRepository notificationTypeRepository, IMapper mapper, IFieldGroupRepository fieldGroupRepository)
        {
            this.fieldRepository = fieldRepository;
            this.levelRepository = levelRepository;
            this.reportReasonRepository = reportReasonRepository;
            this.notificationTypeRepository = notificationTypeRepository;
            this.mapper = mapper;
            this.fieldGroupRepository = fieldGroupRepository;
        }

        public TableResultJson<FieldViewModel> GetAllField(TableRequest request)
        {
            var dataSource = fieldRepository.GetAll().OrderByDescending(x=>x.CreatedDate);
            var dataSourceViewModel = mapper.Map<List<FieldViewModel>>(dataSource).AsEnumerable();
            var response = new TableResultJson<FieldViewModel>
            {
                draw = request.draw,
                recordsFiltered = dataSourceViewModel.Count()
            };
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();
            response.data.ForEach(x => { x.Index = response.data.IndexOf(x) + request.start + 1; });
            return response;
        }

        public async Task<FieldViewModel> GetFieldById(string fieldId)
        {
            var field = await fieldRepository.GetByIdAsync(ObjectId.Parse(fieldId));
            if (field != null)
                return mapper.Map<FieldViewModel>(field);
            return null;
        }

        public TableResultJson<LevelViewModel> GetAllLevel(TableRequest request)
        {
            var dataSource = levelRepository.GetAll();
            var dataSourceViewModel = mapper.Map<List<LevelViewModel>>(dataSource).AsEnumerable();
            var response = new TableResultJson<LevelViewModel>
            {
                draw = request.draw,
                recordsFiltered = dataSourceViewModel.Count()
            };
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();
            response.data.ForEach(x => { x.Index = response.data.IndexOf(x) + request.start + 1; });
            return response;
        }

        public async Task<LevelViewModel> GetLevelById(string levelId)
        {
            var level = await levelRepository.GetByIdAsync(ObjectId.Parse(levelId));
            if (level != null)
                return mapper.Map<LevelViewModel>(level);
            return null;
        }

        public TableResultJson<FieldGroupViewModel> GetAllFieldGroup(TableRequest request)
        {
            var dataSource = fieldGroupRepository.GetAll();
            var dataSourceViewModel = mapper.Map<List<FieldGroupViewModel>>(dataSource).AsEnumerable();
            var response = new TableResultJson<FieldGroupViewModel>()
            {
                draw = request.draw,
                recordsFiltered = dataSourceViewModel.Count()
            };
            dataSourceViewModel = dataSourceViewModel.Skip(request.start).Take(request.length);
            response.data = dataSourceViewModel.ToList();
            response.data.ForEach(x =>
            {
                x.Index = response.data.IndexOf(x) + request.start + 1;
            });
            return response;
        }

        public async Task<LevelViewModel> UpdateLevel(Level entity)
        {
            var existLevel = await levelRepository.GetByIdAsync(ObjectId.Parse(entity.OId));
            if (existLevel == null)
                throw new Exception("Không tìm thấy level cần cập nhật. ");
            var allLevel = levelRepository.GetAll().Where(y => y.Order != existLevel.Order).Select(x => x.Order);
            if (allLevel.Any(x => x == entity.Order))
                throw new Exception("Độ ưu tiên không hợp lệ. ");
            existLevel.Description = entity.Description;
            existLevel.Name = entity.Name;
            existLevel.Order = entity.Order;
            existLevel.ModifiedDate = DateTime.Now;
            existLevel.IsActive = entity.IsActive;
            await levelRepository.UpdateAsync(existLevel, existLevel.Id);
            return mapper.Map<LevelViewModel>(existLevel);
        }

        public async Task<LevelViewModel> AddLevel(Level entity)
        {
            var allLevel = levelRepository.GetAll().Select(x => x.Order);
            if (allLevel.Any(x => x == entity.Order))
                throw new Exception("Độ ưu tiên không hợp lệ. ");
            var data = new Level
            {
                Name = entity.Name,
                Description = entity.Description,
                Order = entity.Order,
                IsActive = entity.IsActive
            };
            await levelRepository.AddAsync(data);
            return mapper.Map<LevelViewModel>(data);
        }

        public async Task<FieldViewModel> UpdateField(Field entity)
        {
            var existField = await fieldRepository.GetByIdAsync(ObjectId.Parse(entity.OId));
            if (existField == null)
                throw new Exception("Không tìm thấy lĩnh vực cần cập nhật. ");
            var allField = fieldRepository.GetAll().Where(x => x.Value != entity.Value);
            if (allField.Any(x => x.Value == entity.Value))
                throw new Exception("Giá trị lĩnh vực đã tồn tại. ");
            existField.Value = entity.Value;
            existField.Status = entity.Status;
            existField.Modified_Date = DateTime.Now;
            existField.CreatedDate = DateTime.Now;
            await fieldRepository.UpdateAsync(existField, existField.Id);
            return mapper.Map<FieldViewModel>(existField);
        }
    }
}

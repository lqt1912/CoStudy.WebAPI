using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class ObjectLevelConvertAction : IMappingAction<ObjectLevel, ObjectLevelViewModel>
    {
        IFieldRepository fieldRepository;
        ILevelRepository levelRepository;

        public ObjectLevelConvertAction(IFieldRepository fieldRepository, ILevelRepository levelRepository)
        {
            this.fieldRepository = fieldRepository;
            this.levelRepository = levelRepository;
        }

        public void Process(ObjectLevel source, ObjectLevelViewModel destination, ResolutionContext context)
        {
            try
            {
                var level = levelRepository.GetById(ObjectId.Parse(source.LevelId));

                if (level == null)
                {
                    throw new Exception("Id level không hợp lệ. ");
                }

                var field = fieldRepository.GetById(ObjectId.Parse(source.FieldId));
                if (field == null)
                {
                    throw new Exception("Id field không hợp lệ. ");
                }

                destination.LevelName = level.Name;
                destination.LevelDescription = level.Description;
                destination.FieldName = field.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}

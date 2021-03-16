using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class Object view model convert action 
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.ObjectLevel, CoStudy.API.Infrastructure.Shared.ViewModels.ObjectLevelViewModel}" />
    public class ObjectLevelConvertAction : IMappingAction<ObjectLevel, ObjectLevelViewModel>
    {
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;
        /// <summary>
        /// The level repository
        /// </summary>
        ILevelRepository levelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLevelConvertAction"/> class.
        /// </summary>
        /// <param name="fieldRepository">The field repository.</param>
        /// <param name="levelRepository">The level repository.</param>
        public ObjectLevelConvertAction(IFieldRepository fieldRepository, ILevelRepository levelRepository)
        {
            this.fieldRepository = fieldRepository;
            this.levelRepository = levelRepository;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(ObjectLevel source, ObjectLevelViewModel destination, ResolutionContext context)
        {
            var level = levelRepository.GetById(ObjectId.Parse(source.LevelId));

            destination.LevelName = level.Name;
            destination.LevelDescription = level.Description;
            destination.FieldName = fieldRepository.GetById(ObjectId.Parse(source.FieldId)).Value;

             

        }
    }
}

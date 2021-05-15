using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    /// <summary>
    /// Class FieldGroupConvertAction
    /// </summary>
    /// <seealso cref="AutoMapper.IMappingAction{CoStudy.API.Domain.Entities.Application.FieldGroup, CoStudy.API.Infrastructure.Shared.ViewModels.FieldGroupViewModel}" />
    public class FieldGroupConvertAction : IMappingAction<FieldGroup, FieldGroupViewModel>
    {
        /// <summary>
        /// The field repository
        /// </summary>
        IFieldRepository fieldRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldGroupConvertAction"/> class.
        /// </summary>
        /// <param name="fieldRepository">The field repository.</param>
        public FieldGroupConvertAction(IFieldRepository fieldRepository)
        {
            this.fieldRepository = fieldRepository;
        }

        /// <summary>
        /// Implementors can modify both the source and destination objects
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public void Process(FieldGroup source, FieldGroupViewModel destination, ResolutionContext context)
        {
            destination.Fields = new List<Field>();
            foreach (var id in source.FieldId)
            {
                var field = fieldRepository.GetById(ObjectId.Parse(id));

                if (field != null)
                {
                    destination.Fields.Add(field);
                }
            }
        }
    }
}

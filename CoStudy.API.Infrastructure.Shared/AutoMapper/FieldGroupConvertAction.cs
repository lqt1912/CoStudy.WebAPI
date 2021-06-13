using System;
using AutoMapper;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class FieldGroupConvertAction : IMappingAction<FieldGroup, FieldGroupViewModel>
    {
        IFieldRepository fieldRepository;


        public FieldGroupConvertAction(IFieldRepository fieldRepository)
        {
            this.fieldRepository = fieldRepository;
        }

        public void Process(FieldGroup source, FieldGroupViewModel destination, ResolutionContext context)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
    }
}

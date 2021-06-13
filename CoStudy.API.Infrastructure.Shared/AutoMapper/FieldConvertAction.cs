using AutoMapper;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class FieldConvertAction : IMappingAction<Field, FieldViewModel>
    {
        public void Process(Field source, FieldViewModel destination, ResolutionContext context)
        {
            //Do nothing
        }
    }
}

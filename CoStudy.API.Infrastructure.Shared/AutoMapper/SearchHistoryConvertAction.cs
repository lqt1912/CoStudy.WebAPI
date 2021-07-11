using AutoMapper;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using CoStudy.API.Infrastructure.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.AutoMapper
{
    public class SearchHistoryConvertAction : IMappingAction<SearchHistory, SearchHistoryViewModel>
    {
        public void Process(SearchHistory source, SearchHistoryViewModel destination, ResolutionContext context)
        {

        }
    }
}

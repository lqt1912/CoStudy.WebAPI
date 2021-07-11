using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class SearchHistoryRepository :BaseRepository<SearchHistory>, ISearchHistoryRepository
    {
        IConfiguration configuration;

        public SearchHistoryRepository(IConfiguration configuration) :base ("search_history", configuration)
        {
            this.configuration = configuration;
        }
    }
}

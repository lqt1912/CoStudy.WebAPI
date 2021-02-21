﻿using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace CoStudy.API.Application.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        IConfiguration configuration;
        public PostRepository(IConfiguration configuration) : base("post", configuration)
        {
            this.configuration = configuration;
        }
    }
}

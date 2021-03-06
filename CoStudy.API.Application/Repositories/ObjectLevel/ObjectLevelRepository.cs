﻿using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    /// <summary>
    /// Class ObjectLevelRepository
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Persistence.Repositories.BaseRepository{CoStudy.API.Domain.Entities.Application.ObjectLevel}" />
    /// <seealso cref="CoStudy.API.Application.Repositories.IObjectLevelRepository" />
    public class ObjectLevelRepository :BaseRepository<ObjectLevel>, IObjectLevelRepository
    {
        /// <summary>
        /// The configuration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLevelRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ObjectLevelRepository(IConfiguration configuration) : base("object_level", configuration)
        {
            this.configuration = configuration;
        }
    }
}

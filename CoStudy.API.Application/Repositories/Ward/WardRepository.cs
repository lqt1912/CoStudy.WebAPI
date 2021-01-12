using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Application.Repositories
{
    public class WardRepository:BaseRepository<Ward>, IWardRepository
    {
        public WardRepository():base("ward")
        {

        }
    }
}

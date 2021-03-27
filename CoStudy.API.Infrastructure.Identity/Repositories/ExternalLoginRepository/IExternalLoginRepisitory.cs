using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using CoStudy.API.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Repositories.ExternalLoginRepository
{
    public interface IExternalLoginRepository :IBaseRepository<ExternalLogin>
    {

    }
}

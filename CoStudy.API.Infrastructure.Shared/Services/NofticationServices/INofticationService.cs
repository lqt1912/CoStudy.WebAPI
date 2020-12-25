﻿using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public interface INofticationService
    {
        Task<AddNofticationResponse> AddNoftication(AddNofticationRequest request);
        List<Noftication> GetCurrentUserNoftication();
    }
}
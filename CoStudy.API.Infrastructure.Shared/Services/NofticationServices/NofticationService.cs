using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Adapters;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.NofticationServices
{
    public class NofticationService: INofticationService
    {
        INofticationRepository nofticationRepository;
        IUserRepository userRepository;
         IHttpContextAccessor contextAccessor;

        public NofticationService(INofticationRepository nofticationRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            this.nofticationRepository = nofticationRepository;
            this.userRepository = userRepository;
            this.contextAccessor = contextAccessor;
        }

        public async Task<AddNofticationResponse> AddNoftication(AddNofticationRequest request)
        {

            var noftication = NofticationAdapter.FromRequest(request);
            await nofticationRepository.AddAsync(noftication);
            return NofticationAdapter.ToResponse(noftication);
        }

        public List<Noftication> GetCurrentUserNoftication()
        {
            var currentUser = Feature.CurrentUser(contextAccessor, userRepository);
            return nofticationRepository.GetAll().Where(x => x.OwnerId == currentUser.Id.ToString()).ToList();

        }
    }
}

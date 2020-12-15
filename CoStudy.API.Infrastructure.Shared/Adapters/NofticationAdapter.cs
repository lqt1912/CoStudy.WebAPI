using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.NofticationResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class NofticationAdapter
    {
        public static Noftication FromRequest(AddNofticationRequest request)
        {
            return new Noftication()
            {
                AuthorId = request.AuthorId,    
                OwnerId = request.OwnerId,
                Content = request.Content,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Status = ItemStatus.Active
            };
        }

        public static AddNofticationResponse ToResponse(Noftication noftication)
        {
            return new AddNofticationResponse()
            {
                Id = noftication.Id.ToString(),
                AuthorId = noftication.AuthorId,
                OwnerId = noftication.OwnerId,
                Content = noftication.Content,
                CreatedDate = noftication.CreatedDate,
                ModifiedDate = noftication.ModifiedDate,
                Status = noftication.Status
            };
        }
    }
}

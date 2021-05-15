using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class MessageAdapter
    {


        public static Conversation FromRequest(AddConversationRequest request, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            var currentUser = Feature.CurrentUser(httpContextAccessor, userRepository);

            var listMember = new List<ConversationMember>();

            foreach (var item in request.Participants)
            {
                item.DateJoin = DateTime.Now;
                item.JoinBy = currentUser.OId;
                listMember.Add(item);
            }

            return new Conversation()
            {
                Name = request.Name,
                Participants = listMember,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

    }
}

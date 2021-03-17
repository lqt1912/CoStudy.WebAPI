using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest;
using CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse;
using Microsoft.AspNetCore.Http;
using System;

namespace CoStudy.API.Infrastructure.Shared.Adapters
{
    public static class MessageAdapter
    {
        public static Message FromRequest(AddMessageRequest request, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {

            if (request.Image != null)
            {
                return new Message()
                {
                    SenderId = Feature.CurrentUser(httpContextAccessor, userRepository).Id.ToString(),
                    MediaContent = request.Image,
                    ConversationId = request.ConversationId,
                    StringContent = request.Content,
                    Status = ItemStatus.Active,
                    MessageType = MessageType.Normal,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
            }
            else
            {
                return new Message()
                {
                    SenderId = Feature.CurrentUser(httpContextAccessor, userRepository).Id.ToString(),
                    MessageType = MessageType.Normal,
                    MediaContent = null,
                    ConversationId = request.ConversationId,
                    StringContent = request.Content,
                    Status = ItemStatus.Active,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now

                };
            }
        }


        public static Conversation FromRequest(AddConversationRequest request)
        {
            return new Conversation()
            {
                Name = request.Name,
                Participants = request.Participants,
                Status = ItemStatus.Active,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

    }
}

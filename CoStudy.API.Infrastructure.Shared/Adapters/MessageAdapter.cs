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
                var imageUrl = "";
                var image = new Image();
                imageUrl = Feature.SaveImage(request.Image, httpContextAccessor, "Message");
                image.ImageUrl = imageUrl;
                image.CreatedDate = DateTime.Now;
                image.ModifiedDate = DateTime.Now;

                return new Message()
                {
                    SenderId = Feature.CurrentUser(httpContextAccessor, userRepository).Id.ToString(),
                    MediaContent = image,
                    ConversationId = request.ConversationId,
                    StringContent = request.Content,
                    Status = ItemStatus.Active,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
            }
            else
            {
                return new Message()
                {
                    SenderId = Feature.CurrentUser(httpContextAccessor, userRepository).Id.ToString(),
                    MediaContent = null,
                    ConversationId = request.ConversationId,
                    StringContent = request.Content,
                    Status = ItemStatus.Active,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now

                };
            }
        }

        public static AddMessageResponse ToResponse(Message message)
        {
            return new AddMessageResponse()
            {
                SenderId = message.SenderId,
                MediaContent = message.MediaContent,
                StringContent = message.StringContent,
                Status = message.Status,
                CreatedDate = message.CreatedDate,
                ModifiedDate = message.ModifiedDate,
                ConversationId = message.ConversationId,
                Id = message.Id.ToString()
            };
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

        public static AddConversationResponse ToResponse(Conversation conversation)
        {
            return new AddConversationResponse()
            {
                Id = conversation.Id.ToString(),
                Name = conversation.Name,
                Participants = conversation.Participants,
                CreatedDate = conversation.CreatedDate,
                Status = conversation.Status
            };
        }
    }
}

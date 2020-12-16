using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
    public class GetMessageByConversationIdResponse
    {
        public GetMessageByConversationIdResponse()
        {
            Messages = new List<Message>();
        }
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }
}

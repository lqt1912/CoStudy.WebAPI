using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
    public class GetMessageByConversationIdResponse
    {
        public GetMessageByConversationIdResponse()
        {
        }
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("messages")]
        public IEnumerable<Message> Messages { get; set; }
    }
}

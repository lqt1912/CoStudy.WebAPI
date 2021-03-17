using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    public class AddConversationRequest
    {
        [JsonPropertyName("participants")]
        public List<ConversationMember> Participants { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

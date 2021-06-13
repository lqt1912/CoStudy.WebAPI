using CoStudy.API.Domain.Entities.Application;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddConversationRequest
    {

        public AddConversationRequest()
        {
            Participants = new List<ConversationMember>();
        }

        [JsonPropertyName("participants")]
        public List<ConversationMember> Participants { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

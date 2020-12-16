using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    public class AddConversationRequest
    {
        [JsonPropertyName("participants")]
        public List<string> Participants { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

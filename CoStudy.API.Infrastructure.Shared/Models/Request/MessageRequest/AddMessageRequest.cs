using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class AddMessageRequest
    {
              [JsonPropertyName("conversation_id")]
        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }

              [JsonProperty("message_type")]
        [JsonPropertyName("message_type")]
        public MessageBaseType MessageType { get; set; }


              [JsonPropertyName("activity_detail")]
        [JsonProperty("activity_detail")]
        public IEnumerable<string> ActivityDetail { get; set; }

              [JsonPropertyName("image")]
        [JsonProperty("image")]
        public IEnumerable<Image> Image { get; set; }

              [JsonProperty("media_url")]
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }


              [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

              [JsonProperty("content")]
        [JsonPropertyName("content")]
        public IEnumerable<string> Content { get; set; }

    }
}

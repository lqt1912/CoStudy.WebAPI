using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class GetMessageByConversationIdRequest : BaseGetAllRequest
    {
              [JsonPropertyName("conversation_id")]
        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }
    }
}

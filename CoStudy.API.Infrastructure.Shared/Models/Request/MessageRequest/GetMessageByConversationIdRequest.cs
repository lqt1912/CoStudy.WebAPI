using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    public class GetMessageByConversationIdRequest
    {
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
    }
}

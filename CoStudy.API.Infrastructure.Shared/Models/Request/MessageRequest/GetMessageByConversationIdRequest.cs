using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    public class GetMessageByConversationIdRequest :BaseGetAllRequest
    {
        [JsonPropertyName("conversation_id")]
        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }

      
    }
}

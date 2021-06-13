using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddOrUpdateCallIdRequest
    {
        [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("call_id")]
        [JsonPropertyName("call_id")]
        public string CallId { get; set; }
    }
}

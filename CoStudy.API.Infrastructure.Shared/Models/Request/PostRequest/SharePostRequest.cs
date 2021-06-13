using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class SharePostRequest : BaseGetAllRequest
    {
              [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

              [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }
    }
}

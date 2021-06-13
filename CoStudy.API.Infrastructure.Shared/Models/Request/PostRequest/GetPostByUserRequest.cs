using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class GetPostByUserRequest : BaseGetAllRequest
    {
              [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}

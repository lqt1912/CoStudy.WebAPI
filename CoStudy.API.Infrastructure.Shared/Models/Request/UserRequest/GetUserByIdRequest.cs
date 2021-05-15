using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class GetUserByIdRequest
    {

        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}

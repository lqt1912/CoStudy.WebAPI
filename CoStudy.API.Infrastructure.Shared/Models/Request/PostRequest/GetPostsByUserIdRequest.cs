using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class GetPostsByUserIdRequest
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}

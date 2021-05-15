using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class GetPostByIdRequest
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }
    }
}

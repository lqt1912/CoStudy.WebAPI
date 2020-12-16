using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class GetPostByIdRequest
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class AddCommentResponse
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

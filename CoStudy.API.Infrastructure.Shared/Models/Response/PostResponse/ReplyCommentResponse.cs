using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class ReplyCommentResponse
    {
        [JsonPropertyName("parent_id")]
        public string ParentCommentId { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

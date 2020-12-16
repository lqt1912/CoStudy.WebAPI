using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class ReplyCommentRequest
    {
        [JsonPropertyName("parent_id")]
        public string ParentCommentId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}

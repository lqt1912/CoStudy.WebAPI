using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public enum ArrangeType
    {
        Ascending,
        Descending
    }

    public enum CommentFilterType
    {
        CreatedDate,
        Upvote
    }

    public class CommentFilterRequest
    {

        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [JsonPropertyName("filter")]
        public CommentFilterType? Filter { get; set; }

        [JsonPropertyName("arrange_type")]
        public ArrangeType? ArrangeType { get; set; }

        [JsonPropertyName("keyword")]
        public string Keyword { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

    }
}

using CoStudy.API.Domain.Entities.Application;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class GetCommentResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [JsonPropertyName("replies_count")]
        public int RepliesCount { get; set; }

        [JsonPropertyName("upvote_count")]
        public int UpvoteCount { get; set; }

        [JsonPropertyName("downvote_count")]
        public int DownvoteCount { get; set; }

        [JsonPropertyName("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonPropertyName("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }
    }
}

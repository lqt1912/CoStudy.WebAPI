using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ReplyCommentViewModel : BaseViewModel
    {


        [JsonProperty("parent_id")]
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }


        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }
        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonProperty("author_field")]
        [JsonPropertyName("author_field")]
        public object  AuthorField { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [JsonProperty("upvote_count")]
        [JsonPropertyName("upvote_count")]
        public int UpvoteCount { get; set; }

        [JsonProperty("downvote_count")]
        [JsonPropertyName("downvote_count")]
        public int DownvoteCount { get; set; }

        [JsonProperty("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }

        [JsonPropertyName("is_vote_by_current")]
        [JsonProperty("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonPropertyName("is_downvote_by_current")]
        [JsonProperty("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }
    }
}

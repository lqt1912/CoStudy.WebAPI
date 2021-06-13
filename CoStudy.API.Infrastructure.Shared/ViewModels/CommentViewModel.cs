using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class CommentViewModel : BaseViewModel
    {

        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonPropertyName("post_id")]
        [JsonProperty("post_id")]
        public string PostId { get; set; }


        [JsonPropertyName("content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("contain_image")]
        [JsonPropertyName("contain_image")]
        public bool ContainImage { get; set; } = false;


        private string _image;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                ContainImage = !string.IsNullOrEmpty(_image);
            }
        }


        [JsonPropertyName("author_id")]
        [JsonProperty("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("author_name")]
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonPropertyName("author_avatar")]
        [JsonProperty("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonProperty("author_email")]
        [JsonPropertyName("author_email")]
        public string  AuthorEmail { get; set; }

        [JsonProperty("author_field")]
        [JsonPropertyName("author_field")]
        public object AuthorField { get; set; }

        [JsonPropertyName("created_date")]
        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        [JsonProperty("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [JsonPropertyName("replies_count")]
        [JsonProperty("replies_count")]
        public int RepliesCount { get; set; } = 0;

        [JsonPropertyName("upvote_count")]
        [JsonProperty("upvote_count")]
        public int UpvoteCount { get; set; } = 0;

        [JsonPropertyName("downvote_count")]
        [JsonProperty("downvote_count")]
        public int DownvoteCount { get; set; } = 0;

        [JsonPropertyName("is_edited")]
        [JsonProperty("is_edited")]
        public bool? IsEdited { get; set; } = false;


        [JsonProperty("is_vote_by_current")]
        [JsonPropertyName("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonProperty("is_downvote_by_current")]
        [JsonPropertyName("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }

    }
}

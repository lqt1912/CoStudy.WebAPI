using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class Comment View  Model
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [JsonPropertyName("post_id")]
        [JsonProperty("post_id")]
        public string PostId { get; set; }


        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonPropertyName("content")]
        [JsonProperty("content")]
        public string Content { get; set; }


        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [JsonPropertyName("author_id")]
        [JsonProperty("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [JsonPropertyName("author_name")]
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        [JsonPropertyName("author_avatar")]
        [JsonProperty("author_avatar")]
        public string AuthorAvatar { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonPropertyName("status")]
        [JsonProperty("status")]
        public ItemStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonPropertyName("created_date")]
        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonPropertyName("modified_date")]
        [JsonProperty("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the replies count.
        /// </summary>
        /// <value>
        /// The replies count.
        /// </value>
        [JsonPropertyName("replies_count")]
        [JsonProperty("replies_count")]
        public int RepliesCount { get; set; }

        /// <summary>
        /// Gets or sets the upvote count.
        /// </summary>
        /// <value>
        /// The upvote count.
        /// </value>
        [JsonPropertyName("upvote_count")]
        [JsonProperty("upvote_count")]
        public int UpvoteCount { get; set; }

        /// <summary>
        /// Gets or sets the downvote count.
        /// </summary>
        /// <value>
        /// The downvote count.
        /// </value>
        [JsonPropertyName("downvote_count")]
        [JsonProperty("downvote_count")]
        public int DownvoteCount { get; set; }

        /// <summary>
        /// Gets or sets the is edited.
        /// </summary>
        /// <value>
        /// The is edited.
        /// </value>
        [JsonPropertyName("is_edited")]
        [JsonProperty("is_edited")]
        public bool? IsEdited { get; set; }


        /// <summary>
        /// Gets or sets the is vote by current.
        /// </summary>
        /// <value>
        /// The is vote by current.
        /// </value>
        [JsonProperty("is_vote_by_current")]
        [JsonPropertyName("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        /// <summary>
        /// Gets or sets the is down vote by current.
        /// </summary>
        /// <value>
        /// The is down vote by current.
        /// </value>
        [JsonProperty("is_downvote_by_current")]
        [JsonPropertyName("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }

    }
}

using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class Reply Comment ViewModel
    /// </summary>
    public class ReplyCommentViewModel
    {
        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        [JsonProperty("parent_id")]
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }


        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string  OId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        /// 
        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }
        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        /// 
        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the upvote count.
        /// </summary>
        /// <value>
        /// The upvote count.
        /// </value>
        [JsonProperty("upvote_count")]
        [JsonPropertyName("upvote_count")]
        public int UpvoteCount { get; set; }

        /// <summary>
        /// Gets or sets the downvote count.
        /// </summary>
        /// <value>
        /// The downvote count.
        /// </value>
        [JsonProperty("downvote_count")]
        [JsonPropertyName("downvote_count")]
        public int DownvoteCount { get; set; }


        /// <summary>
        /// Gets or sets the is edited.
        /// </summary>
        /// <value>
        /// The is edited.
        /// </value>
        [JsonProperty("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }

        /// <summary>
        /// Gets or sets the is vote by current.
        /// </summary>
        /// <value>
        /// The is vote by current.
        /// </value>
        [JsonPropertyName("is_vote_by_current")]
        [JsonProperty("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        /// <summary>
        /// Gets or sets the is down vote by current.
        /// </summary>
        /// <value>
        /// The is down vote by current.
        /// </value>
        [JsonPropertyName("is_downvote_by_current")]
        [JsonProperty("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }
    }
}

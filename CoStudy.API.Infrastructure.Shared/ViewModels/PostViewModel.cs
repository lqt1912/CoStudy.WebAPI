using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class Post View Model
    /// </summary>
    public class PostViewModel
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
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }



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
        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }


        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        /// <summary>
        /// Gets or sets the upvote.
        /// </summary>
        /// <value>
        /// The upvote.
        /// </value>
        [JsonProperty("upvote")]
        [JsonPropertyName("upvote")]
        public int Upvote { get; set; }

        /// <summary>
        /// Gets or sets the downvote.
        /// </summary>
        /// <value>
        /// The downvote.
        /// </value>
        [JsonProperty("downvote")]
        [JsonPropertyName("downvote")]
        public int Downvote { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the string contents.
        /// </summary>
        /// <value>
        /// The string contents.
        /// </value>
        [JsonProperty("string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        /// <summary>
        /// Gets or sets the media contents.
        /// </summary>
        /// <value>
        /// The media contents.
        /// </value>
        [JsonProperty("image_contents")]
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }


        /// <summary>
        /// Gets or sets the comment count.
        /// </summary>
        /// <value>
        /// The comment count.
        /// </value>
        [JsonProperty("comment_count")]
        [JsonPropertyName("comments_count")]
        public int CommentCount { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [JsonProperty("is_vote_by_current")]
        [JsonPropertyName("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonProperty("is_downvote_by_current")]
        [JsonPropertyName("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }

    }
}


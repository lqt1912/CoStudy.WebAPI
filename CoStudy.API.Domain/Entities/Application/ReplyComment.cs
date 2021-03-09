using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Reply comment. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class ReplyComment : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyComment"/> class.
        /// </summary>
        public ReplyComment() : base()
        {

        }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        [BsonElement("parent_id")]
        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [BsonElement("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the upvote count.
        /// </summary>
        /// <value>
        /// The upvote count.
        /// </value>
        [BsonElement("upvote_count")]
        [JsonPropertyName("upvote_count")]
        public int UpvoteCount { get; set; }

        /// <summary>
        /// Gets or sets the downvote count.
        /// </summary>
        /// <value>
        /// The downvote count.
        /// </value>
        [BsonElement("downvote_count")]
        [JsonPropertyName("downvote_count")]
        public int DownvoteCount { get; set; }


        /// <summary>
        /// Gets or sets the is edited.
        /// </summary>
        /// <value>
        /// The is edited.
        /// </value>
        [BsonElement("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }

        /// <summary>
        /// Gets or sets the is vote by current.
        /// </summary>
        /// <value>
        /// The is vote by current.
        /// </value>
        [JsonPropertyName("is_vote_by_current")]
        [BsonElement("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        /// <summary>
        /// Gets or sets the is down vote by current.
        /// </summary>
        /// <value>
        /// The is down vote by current.
        /// </value>
        [JsonPropertyName("is_downvote_by_current")]
        [BsonElement("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }
    }
}

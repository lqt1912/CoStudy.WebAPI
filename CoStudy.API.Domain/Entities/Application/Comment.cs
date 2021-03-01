using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class comment
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Comment : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        public Comment() : base()
        {
            Replies = new List<ReplyComment>();
        }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [BsonElement("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

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
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [BsonElement("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

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
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [BsonElement("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        [BsonElement("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

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
        /// Gets or sets the replies.
        /// </summary>
        /// <value>
        /// The replies.
        /// </value>
        [BsonElement("replies")]
        [JsonPropertyName("replies")]
        public List<ReplyComment> Replies { get; set; }

        /// <summary>
        /// Gets or sets the replies count.
        /// </summary>
        /// <value>
        /// The replies count.
        /// </value>
        [BsonElement("replies_count")]
        [JsonPropertyName("replies_count")]
        public int RepliesCount { get; set; }

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
        /// Use for response
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

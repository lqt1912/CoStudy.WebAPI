using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Notification.
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Noftication : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Noftication" /> class.
        /// </summary>
        public Noftication() : base()
        {
        }

        /// <summary>
        /// Người tạo ra.  (Người vote, upvote, ...)
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Chủ sở hữu đối tượng.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [BsonElement("owner_id")]
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }


        /// <summary>
        /// Người nhận thông báo
        /// </summary>
        /// <value>
        /// The receiver identifier.
        /// </value>
        [BsonElement("receiver_id")]
        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [BsonElement("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        public ContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [BsonElement("object_id")]
        [JsonPropertyName("object_id")]
        public string  ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

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
        /// Gets or sets the is read.
        /// </summary>
        /// <value>
        /// The is read.
        /// </value>
        [BsonElement("is_read")]
        [JsonPropertyName("is_read")]
        public bool? IsRead { get; set; }

    }
    /// <summary>
    /// Enum ContentType
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// The add post notify
        /// </summary>
        ADD_POST_NOTIFY,
        /// <summary>
        /// The upvote post notify
        /// </summary>
        UPVOTE_POST_NOTIFY,
        /// <summary>
        /// The downvote post notify
        /// </summary>
        DOWNVOTE_POST_NOTIFY,
        /// <summary>
        /// The upvote comment notify
        /// </summary>
        UPVOTE_COMMENT_NOTIFY,
        /// <summary>
        /// The downvote comment notify
        /// </summary>
        DOWNVOTE_COMMENT_NOTIFY,
        /// <summary>
        /// The upvote reply notify
        /// </summary>
        UPVOTE_REPLY_NOTIFY,
        /// <summary>
        /// The downvote reply notify
        /// </summary>
        DOWNVOTE_REPLY_NOTIFY,
        /// <summary>
        /// The follow notify
        /// </summary>
        FOLLOW_NOTIFY,

        /// <summary>
        /// The comment notify
        /// </summary>
        COMMENT_NOTIFY,

        /// <summary>
        /// The reply comment notify
        /// </summary>
        REPLY_COMMENT_NOTIFY

    }
}

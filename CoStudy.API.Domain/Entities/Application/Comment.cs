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
        /// Gets or sets the is edited.
        /// </summary>
        /// <value>
        /// The is edited.
        /// </value>
        [BsonElement("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }
      
    }
}

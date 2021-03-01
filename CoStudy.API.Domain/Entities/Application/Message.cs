using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Message
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Message : Entity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message() : base()
        {

        }

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>
        [BsonElement("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }


        /// <summary>
        /// Gets or sets the conversation identifier.
        /// </summary>
        /// <value>
        /// The conversation identifier.
        /// </value>
        [BsonElement("conversation_id")]
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        /// <summary>
        /// Gets or sets the content of the media.
        /// </summary>
        /// <value>
        /// The content of the media.
        /// </value>
        [BsonElement("media_content")]
        [JsonPropertyName("media_content")]
        public Image MediaContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the string.
        /// </summary>
        /// <value>
        /// The content of the string.
        /// </value>
        [BsonElement("string_content")]
        [JsonPropertyName("string_content")]
        public string StringContent { get; set; }

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
        /// Delete or not ?
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

    }
}

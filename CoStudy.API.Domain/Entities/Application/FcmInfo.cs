using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class FCM info 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class FcmInfo : Entity
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [BsonElement("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>
        [BsonElement("device_token")]
        [JsonPropertyName("device_token")]
        public string DeviceToken { get; set; }

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

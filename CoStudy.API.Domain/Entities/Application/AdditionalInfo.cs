using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Additional info class
    /// </summary>
    public class AdditionalInfo
    {


        /// <summary>
        /// Gets or sets the type of the information.
        /// </summary>
        /// <value>
        /// The type of the information.
        /// </value>
        [BsonElement("info_type")]
        [JsonPropertyName("info_type")]
        public InfoType InfoType { get; set; }

        /// <summary>
        /// Gets or sets the information value.
        /// </summary>
        /// <value>
        /// The information value.
        /// </value>
        [BsonElement("info_value")]
        [JsonPropertyName("info_value")]
        public string InfoValue { get; set; }

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

using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class image
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Image : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image() : base()
        {

        }

        /// <summary>
        /// Gets or sets the discription.
        /// </summary>
        /// <value>
        /// The discription.
        /// </value>
        [BsonElement("discription")]
        [JsonPropertyName("discription")]
        public string Discription { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [BsonElement("image_url")]
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image hash.
        /// </summary>
        /// <value>
        /// The image hash.
        /// </value>
        [BsonElement("image_hash")]
        [JsonPropertyName("image_hash")]
        public string ImageHash { get; set; }

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

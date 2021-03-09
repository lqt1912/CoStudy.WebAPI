using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Media content. 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class MediaContent : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaContent"/> class.
        /// </summary>
        public MediaContent() : base()
        {

        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [BsonElement("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail image.
        /// </summary>
        /// <value>
        /// The thumbnail image.
        /// </value>
        [BsonElement("thumbnail_image")]
        public Image ThumbnailImage { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}

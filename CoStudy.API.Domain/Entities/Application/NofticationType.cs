using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class NotificationType
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class NotificationType :Entity
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [BsonElement("code")]
        public string  Code { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        [BsonElement("object_type")]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>
        /// The content template.
        /// </value>
        [BsonElement("content_template")]
        public string  ContentTemplate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [BsonElement("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [BsonElement("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}

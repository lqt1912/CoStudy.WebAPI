using CoStudy.API.Domain.Entities.BaseEntity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Post Content 
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class PostContent : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostContent"/> class.
        /// </summary>
        public PostContent() : base()
        {

        }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [BsonElement("content_type")]
        [JsonPropertyName("content_type")]
        [FromForm(Name = "content_type")]
        public PostContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [BsonElement("content")]
        [JsonPropertyName("content")]
        [FromForm(Name = "content")]
        public string Content { get; set; }

    }
}

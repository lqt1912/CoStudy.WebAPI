using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class MessageMultiMedia
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.Application.MessageBase" />
    public class MessageMultiMedia:MessageBase
    {
        /// <summary>
        /// Gets or sets the media URL.
        /// </summary>
        /// <value>
        /// The media URL.
        /// </value>
        [BsonElement("media_url")]
        public string  MediaUrl { get; set; }
    }
}

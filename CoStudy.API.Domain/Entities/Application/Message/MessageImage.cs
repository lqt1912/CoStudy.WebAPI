using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class MessageImage
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.Application.MessageBase" />
    public class MessageImage:MessageBase
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [BsonElement("image")]
        public IEnumerable<Image> Image { get; set; }
    }
}

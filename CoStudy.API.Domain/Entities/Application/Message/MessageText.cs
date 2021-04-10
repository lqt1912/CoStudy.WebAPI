using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class MessageText
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.Application.MessageBase" />
    public class MessageText : MessageBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [BsonElement("content")]
        public IEnumerable<string> Content { get; set; }
    }
}

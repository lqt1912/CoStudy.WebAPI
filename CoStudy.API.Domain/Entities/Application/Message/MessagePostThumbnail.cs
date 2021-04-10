using MongoDB.Bson.Serialization.Attributes;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class MessagePostThumbnail
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.Application.MessageBase" />
    public class MessagePostThumbnail :MessageBase
    {
        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [BsonElement("post_id")]
        public string PostId { get; set; }
    }
}

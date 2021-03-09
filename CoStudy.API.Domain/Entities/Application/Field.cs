using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Field
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Field : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field() : base()
        {

        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [BsonElement("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}

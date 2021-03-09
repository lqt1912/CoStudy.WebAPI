using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.BaseEntity
{
    /// <summary>
    /// Class Entity
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonIgnore]
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonPropertyName("oid")]
        [BsonElement("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            Id = ObjectId.GenerateNewId();
            OId = Id.ToString();
        }
    }
}

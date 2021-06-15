using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.BaseEntity
{
    public class Entity
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId Id { get; set; }

        [JsonPropertyName("oid")]
        [BsonElement("oid")]
        public string OId { get; set; }

        public Entity()
        {
            Id = ObjectId.GenerateNewId();
            OId = Id.ToString();
        }
    }
}

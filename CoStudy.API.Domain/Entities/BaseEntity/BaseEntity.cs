using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoStudy.API.Domain.Entities.BaseEntity
{
    public class Entity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public Entity()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Entities.Demo
{
    public class MyEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public MyEntity()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}

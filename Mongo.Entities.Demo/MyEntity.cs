using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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

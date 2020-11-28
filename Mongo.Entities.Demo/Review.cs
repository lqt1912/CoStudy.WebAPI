using Mongo.Entities.Demo;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Examples.Models
{
    public class Review:MyEntity
    {
        public Review():base()
        {

        }

        [BsonElement("star")]
        public int Stars { get; set; }

        [BsonElement("reviewer")]
        public string Reviewer { get; set; }
    }
}

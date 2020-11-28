using Mongo.Entities.Demo;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Examples.Models
{
    public class Author : MyEntity
    {
        public Author() :base()
        {
            Reviews = new List<Review>();
        }

        [BsonElement("name")]
        public string Name { get; set; }


        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("reviews")]

        public List<Review> Reviews  { get; set; }
   
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using System.Collections.Generic;

namespace Examples.Models
{
    public class Book : Entity
    {
        public Book() : base()
        {
            Authors = new List<Author>();
        }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("authors")]
        public List<Author> Authors { get; set; }
    }
}

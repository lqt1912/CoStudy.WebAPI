using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Comment :Entity
    {
        public Comment():base()
        {
            Replies = new List<Comment>();
        }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("replies")]
        public  List<Comment> Replies { get; set; }
    }
}

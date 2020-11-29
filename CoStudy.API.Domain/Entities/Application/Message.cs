using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Message:Entity
    {

        public Message() : base()
        {

        }

        [BsonElement("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("is_read_by_author")]
        public bool IsReadByAuthor { get; set; }

        [BsonElement("is_read_by_guest")]
        public bool IsReadByGuest { get; set; }


        [BsonElement("media_content")]
        public MediaContent Content { get; set; }

        /// <summary>
        /// Delete or not ?
        /// </summary>
        /// 
        [BsonElement("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

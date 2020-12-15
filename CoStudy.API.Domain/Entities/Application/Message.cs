using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Message:Entity
    {

        public Message() : base()
        {

        }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("is_read_by_author")]
        [JsonPropertyName("is_read_by_author")]
        public bool IsReadByAuthor { get; set; }

        [BsonElement("is_read_by_guest")]
        [JsonPropertyName("is_read_by_guest")]
        public bool IsReadByGuest { get; set; }


        [BsonElement("media_content")]
        [JsonPropertyName("media_content")]
        public MediaContent Content { get; set; }

        /// <summary>
        /// Delete or not ?
        /// </summary>
        /// 
        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Noftication : Entity
    {
        public Noftication() : base()
        {
        }

        [BsonElement("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [BsonElement("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [BsonElement("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [BsonElement("owner_id")]
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("is_read")]
        [JsonPropertyName("is_read")]
        public bool? IsRead { get; set; }
    }
}

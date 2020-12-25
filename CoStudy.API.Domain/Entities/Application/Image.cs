using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Image : Entity
    {
        public Image() : base()
        {

        }

        [BsonElement("discription")]
        [JsonPropertyName("discription")]
        public string Discription { get; set; }

        [BsonElement("image_url")]
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [BsonElement("image_hash")]
        [JsonPropertyName("image_hash")]
        public string ImageHash { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

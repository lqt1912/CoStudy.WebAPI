using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Field : Entity
    {
        public Field() : base()
        {

        }

        [BsonElement("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [BsonElement("thumbnail_image")]
        [JsonPropertyName("thumbnail_image")]
        public Image ThumbnailImage { get; set; }


        [BsonElement("thumbnail_image_hash")]
        [JsonPropertyName("thumbnail_image_hash")]
        public string ThumbnailImageHash { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }


    }
}

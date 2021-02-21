using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class ObjectLevel : Entity
    {
        [BsonElement("object_id")]
        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        [BsonElement("field_id")]
        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }

        [BsonElement("level_id")]
        [JsonPropertyName("level_id")]
        public string LevelId { get; set; }

        [BsonElement("point")]
        [JsonPropertyName("point")]
        public int? Point { get; set; } = 0;

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [BsonElement("is_active")]
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;
    }
}

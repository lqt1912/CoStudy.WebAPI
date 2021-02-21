using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Level : Entity
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [BsonElement("order")]
        [JsonPropertyName("order")]
        public int? Order { get; set; }


        [BsonElement("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        [BsonElement("is_active")]
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; } = true;
    }
}

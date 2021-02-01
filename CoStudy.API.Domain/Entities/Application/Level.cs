using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Level :Entity
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [BsonElement("ison")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}

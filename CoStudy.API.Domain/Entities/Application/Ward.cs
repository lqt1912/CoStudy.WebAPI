using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Ward
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [JsonPropertyName("code")]
        [BsonElement("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        [BsonElement("name")]
        public string Name { get; set; }

        [JsonPropertyName("district_code")]
        [BsonElement("district_code")]
        public string DistrictCode { get; set; }

    }
}

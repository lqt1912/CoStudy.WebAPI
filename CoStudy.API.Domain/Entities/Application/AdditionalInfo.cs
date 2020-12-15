using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class AdditionalInfo { 


        [BsonElement("info_type")]
        [JsonPropertyName("info_type")]
        public InfoType InfoType { get; set; }

        [BsonElement("info_value")]
        [JsonPropertyName("info_value")]
        public string InfoValue { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

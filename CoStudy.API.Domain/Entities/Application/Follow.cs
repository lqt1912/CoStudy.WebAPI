using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Follow:Entity
    {
        [BsonElement("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [BsonElement("avatar")]
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [BsonElement("follow_date")]
        [JsonPropertyName("follow_date")]
        public DateTime? FollowDate { get; set; }


        [BsonElement("from_id")]
        [JsonPropertyName("from_id")]
        public string  FromId { get; set; }

        [BsonElement("to_id")]
        public string ToId { get; set; }
    }
}

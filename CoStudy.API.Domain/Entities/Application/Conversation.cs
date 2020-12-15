using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Conversation:Entity
    {
        public Conversation():base()
        {
            Messages = new List<Message>();
        }

        [BsonElement("host_id")]
        [JsonPropertyName("host_id")]
        public string HostId { get; set; }

        [BsonElement("guest_id")]
        [JsonPropertyName("guest_id")]
        public string GuestId { get; set; }


        [BsonElement("messages")]
        [JsonPropertyName("messages")]
        public  List<Message> Messages { get; set; }

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

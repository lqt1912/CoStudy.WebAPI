using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Message : Entity
    {

        public Message() : base()
        {

        }

        [BsonElement("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }


        [BsonElement("conversation_id")]
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        [BsonElement("media_content")]
        [JsonPropertyName("media_content")]
        public Image MediaContent { get; set; }

        [BsonElement("string_content")]
        [JsonPropertyName("string_content")]
        public string StringContent { get; set; }


        /// <summary>
        /// Delete or not ?
        /// </summary>
        /// 
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

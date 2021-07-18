using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class MessageViewModel
    {
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }


        [JsonProperty("conversation_id")]
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }

        [JsonProperty("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonProperty("message_type")]
        [JsonPropertyName("message_type")]
        public MessageBaseType? MessageType { get; set; }


        [JsonProperty("sender_name")]
        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonProperty("sender_avatar")]
        [JsonPropertyName("sender_avatar")]
        public string SenderAvatar { get; set; }

        [JsonProperty("receiver_name")]
        [JsonPropertyName("receiver_name")]
        public string ReceiverName { get; set; }

        [JsonProperty("receiver_avatar")]
        [JsonPropertyName("receiver_avatar")]
        public string ReceiverAvatar { get; set; }

        [JsonProperty("middle_name")]
        [JsonPropertyName("middle_name")]
        public string  MiddleName { get; set; }

        [JsonProperty("middle_avatar")]
        [JsonPropertyName("middle_avatar")]
        public string  MiddleAvatar { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public object Content { get; set; }

        [JsonProperty("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }


        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [JsonPropertyName("created_date")]
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }


        [JsonPropertyName("modified_date")]
        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}

using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.MessageResponse
{
    public class AddMessageResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }


        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }

        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        [JsonPropertyName("media_content")]
        public Image MediaContent { get; set; }

        [JsonPropertyName("string_content")]
        public string StringContent { get; set; }


        /// <summary>
        /// Delete or not ?
        /// </summary>
        /// 
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}

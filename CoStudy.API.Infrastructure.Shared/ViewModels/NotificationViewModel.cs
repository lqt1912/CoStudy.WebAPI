using CoStudy.API.Application.FCM;
using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class NotificationViewModel
    {
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonProperty("owner_id")]
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("object_id")]
        [JsonProperty("object_id")]
        public string ObjectId { get; set; }
        [JsonPropertyName("created_date")]
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonPropertyName("status")]
        [JsonProperty("status")]
        public ItemStatus Status { get; set; }

        [JsonPropertyName("is_read")]
        [JsonProperty("is_read")]
        public bool? IsRead { get; set; }

        [JsonProperty("notification_type")]
        [JsonPropertyName("notification_type")]
        public PushedNotificationType NotificationType { get; set; }
    }
}

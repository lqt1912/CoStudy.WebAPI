using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Application.FCM
{
    public class PushedNotificationViewModel
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
        public bool? IsRead { get; set; } = false;

        public PushedNotificationType? NotificationType { get; set; } = PushedNotificationType.Other;

    }

    public enum PushedNotificationType
    {
        Post,
        Comment,
        Reply,
        User,
        Other
    }
}



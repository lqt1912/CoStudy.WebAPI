using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Application.FCM
{
    public class PushedNotificationViewModel
    {
        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the author avatar.
        /// </summary>
        /// <value>
        /// The author avatar.
        /// </value>
        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("owner_id")]
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("object_id")]
        [JsonProperty("object_id")]
        public string ObjectId { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonPropertyName("created_date")]
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonPropertyName("modified_date")]
        [JsonProperty("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonPropertyName("status")]
        [JsonProperty("status")]
        public ItemStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the is read.
        /// </summary>
        /// <value>
        /// The is read.
        /// 
        /// </value>
        [JsonPropertyName("is_read")]
        [JsonProperty("is_read")]
        public bool? IsRead { get; set; } = false;


    }
}

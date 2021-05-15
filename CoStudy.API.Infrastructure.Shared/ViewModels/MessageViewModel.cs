using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class MessageViewModel.
    /// </summary>
    public class MessageViewModel
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
        /// Gets or sets the conversation identifier.
        /// </summary>
        /// <value>
        /// The conversation identifier.
        /// </value>
        [JsonProperty("conversation_id")]
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>
        [JsonProperty("sender_id")]
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [JsonProperty("message_type")]
        [JsonPropertyName("message_type")]
        public MessageBaseType? MessageType { get; set; }


        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        /// <value>
        /// The name of the sender.
        /// </value>
        [JsonProperty("sender_name")]
        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the sender avatar.
        /// </summary>
        /// <value>
        /// The sender avatar.
        /// </value>
        [JsonProperty("sender_avatar")]
        [JsonPropertyName("sender_avatar")]
        public string SenderAvatar { get; set; }

        /// <summary>
        /// Gets or sets the name of the receiver.
        /// </summary>
        /// <value>
        /// The name of the receiver.
        /// </value>
        [JsonProperty("receiver_name")]
        [JsonPropertyName("receiver_name")]
        public string ReceiverName { get; set; }

        /// <summary>
        /// Gets or sets the receiver avatar.
        /// </summary>
        /// <value>
        /// The receiver avatar.
        /// </value>
        [JsonProperty("receiver_avatar")]
        [JsonPropertyName("receiver_avatar")]
        public string ReceiverAvatar { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the is edited.
        /// </summary>
        /// <value>
        /// The is edited.
        /// </value>
        [JsonProperty("is_edited")]
        [JsonPropertyName("is_edited")]
        public bool? IsEdited { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

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
    }
}

using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    /// <summary>
    /// Class AddMessageRequest
    /// </summary>
    public class AddMessageRequest
    {
        /// <summary>
        /// Gets or sets the conversation identifier.
        /// </summary>
        /// <value>
        /// The conversation identifier.
        /// </value>
        [JsonPropertyName("conversation_id")]
        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [JsonProperty("message_type")]
        [JsonPropertyName("message_type")]
        public MessageBaseType MessageType { get; set; }


        /// <summary>
        /// Gets or sets the activity detail.
        /// </summary>
        /// <value>
        /// The activity detail.
        /// </value>
        [JsonPropertyName("activity_detail")]
        [JsonProperty("activity_detail")]
        public IEnumerable<string> ActivityDetail { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [JsonPropertyName("image")]
        [JsonProperty("image")]
        public IEnumerable<Image> Image { get; set; }

        /// <summary>
        /// Gets or sets the media URL.
        /// </summary>
        /// <value>
        /// The media URL.
        /// </value>
        [JsonProperty("media_url")]
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }


        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public IEnumerable<string> Content { get; set; }

    }
}

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class GetMessageByConversationIdRequest
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class GetMessageByConversationIdRequest : BaseGetAllRequest
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
    }
}

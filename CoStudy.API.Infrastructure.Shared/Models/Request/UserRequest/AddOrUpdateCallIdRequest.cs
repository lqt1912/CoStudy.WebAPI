using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class AddOrUpdateCallIdRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the call identifier.
        /// </summary>
        /// <value>
        /// The call identifier.
        /// </value>
        [JsonProperty("call_id")]
        [JsonPropertyName("call_id")]
        public string CallId { get; set; }
    }
}

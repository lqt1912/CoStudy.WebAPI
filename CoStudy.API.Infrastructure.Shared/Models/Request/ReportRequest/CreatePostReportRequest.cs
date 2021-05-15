using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class CreatePostRequest
    /// </summary>
    public class CreatePostReportRequest
    {
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
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        [JsonProperty("reason")]
        [JsonPropertyName("reason")]
        public IEnumerable<string> Reason { get; set; }


        /// <summary>
        /// Gets or sets the external reason.
        /// </summary>
        /// <value>
        /// The external reason.
        /// </value>
        [JsonProperty("external_reason")]
        [JsonPropertyName("external_reason")]
        public string ExternalReason { get; set; }
    }
}

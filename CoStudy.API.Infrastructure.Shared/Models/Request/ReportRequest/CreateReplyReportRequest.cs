using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class CreateReplyReportRequest
    {
        [JsonProperty("reply_id")]
        [JsonPropertyName("reply_id")]
        public string ReplyId { get; set; }


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

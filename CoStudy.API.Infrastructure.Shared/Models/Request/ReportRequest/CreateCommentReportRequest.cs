using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class CreateCommentPostRequest
    /// </summary>
    public class CreateCommentReportRequest
    {
        /// <summary>
        /// Gets or sets the comment identifier.
        /// </summary>
        /// <value>
        /// The comment identifier.
        /// </value>
        [JsonProperty("comment_id")]
        [JsonPropertyName("comment_id")]
        public string CommentId { get; set; }

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

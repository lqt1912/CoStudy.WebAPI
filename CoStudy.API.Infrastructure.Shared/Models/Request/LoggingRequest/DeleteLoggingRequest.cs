using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.LoggingRequest
{
    /// <summary>
    /// Class DeleteLoggingRequest
    /// </summary>
    public class DeleteLoggingRequest
    {
        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        /// <value>
        /// The ids.
        /// </value>
        [JsonProperty("ids")]
        [JsonPropertyName("ids")]
        public IEnumerable<string> Ids { get; set; }

    }
}

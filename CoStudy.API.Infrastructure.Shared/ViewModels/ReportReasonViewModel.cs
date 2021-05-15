using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class ReportReasonViewModel
    /// </summary>
    public class ReportReasonViewModel
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
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        [JsonProperty("detail")]
        [JsonPropertyName("detail")]
        public string Detail { get; set; }


        [JsonProperty("created_by")]
        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("created_by_name")]
        [JsonPropertyName("created_by_name")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class ReportViewModel
    /// </summary>
    public class ReportViewModel
    {

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
        /// Gets or sets the name of the auhthor.
        /// </summary>
        /// <value>
        /// The name of the auhthor.
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
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [JsonProperty("object_id")]
        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        [JsonProperty("object_type")]
        [JsonPropertyName("object_type")]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the report reason.
        /// </summary>
        /// <value>
        /// The report reason.
        /// </value>
        [JsonProperty("report_reason")]
        [JsonPropertyName("report_reason")]
        public List<ReportReasonViewModel> ReportReason { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is approve.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is approve; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("is_approved")]
        [JsonPropertyName("is_approved")]
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the name of the approve status.
        /// </summary>
        /// <value>
        /// The name of the approve status.
        /// </value>
        [JsonProperty("approve_status")]
        [JsonPropertyName("approve_status")]
        public string ApproveStatusName { get; set; }

        /// <summary>
        /// Gets or sets the approved by.
        /// </summary>
        /// <value>
        /// The approved by.
        /// </value>
        [JsonProperty("approved_by")]
        [JsonPropertyName("approved_by")]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the approved by.
        /// </summary>
        /// <value>
        /// The name of the approved by.
        /// </value>
        [JsonProperty("approved_by_name")]
        [JsonPropertyName("approved_by_name")]
        public string ApprovedByName { get; set; }

        /// <summary>
        /// Gets or sets the approve date.
        /// </summary>
        /// <value>
        /// The approve date.
        /// </value>
        [JsonProperty("approve_date")]
        [JsonPropertyName("approve_date")]
        public DateTime? ApproveDate { get; set; }
    }
}

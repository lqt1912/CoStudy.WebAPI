using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ReportViewModel
    {
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId  { get; set; }

        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonPropertyName("author_email")]
        [JsonProperty("author_email")]
        public string  AuthorEmail{ get; set; }

        [JsonProperty("object_id")]
        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        [JsonProperty("object_type")]
        [JsonPropertyName("object_type")]
        public string ObjectType { get; set; }

        [JsonProperty("report_reason")]
        [JsonPropertyName("report_reason")]
        public List<ReportReasonViewModel> ReportReason { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [JsonProperty("is_approved")]
        [JsonPropertyName("is_approved")]
        public bool IsApproved { get; set; }

        [JsonProperty("approve_status")]
        [JsonPropertyName("approve_status")]
        public string ApproveStatusName { get; set; }

        [JsonProperty("approved_by")]
        [JsonPropertyName("approved_by")]
        public string ApprovedBy { get; set; }

        [JsonProperty("approved_by_name")]
        [JsonPropertyName("approved_by_name")]
        public string ApprovedByName { get; set; }

        [JsonProperty("approve_date")]
        [JsonPropertyName("approve_date")]
        public DateTime? ApproveDate { get; set; }
    }
}

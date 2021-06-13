using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
       public class ReportReasonViewModel
    {
              [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

              [JsonProperty("detail")]
        [JsonPropertyName("detail")]
        public string Detail { get; set; }


        [JsonProperty("created_by")]
        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("created_by_name")]
        [JsonPropertyName("created_by_name")]
        public string CreatedByName { get; set; }

              [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

              [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}

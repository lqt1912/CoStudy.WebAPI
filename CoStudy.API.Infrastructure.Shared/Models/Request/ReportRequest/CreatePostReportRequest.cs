using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class CreatePostReportRequest
    {
              [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }


              [JsonProperty("reason")]
        [JsonPropertyName("reason")]
        public IEnumerable<string> Reason { get; set; }


              [JsonProperty("external_reason")]
        [JsonPropertyName("external_reason")]
        public string ExternalReason { get; set; }
    }
}

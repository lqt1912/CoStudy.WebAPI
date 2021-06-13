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


              [JsonProperty("reason")]
        [JsonPropertyName("reason")]
        public IEnumerable<string> Reason { get; set; }


              [JsonProperty("external_reason")]
        [JsonPropertyName("external_reason")]
        public string ExternalReason { get; set; }
    }
}

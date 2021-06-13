using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class CreateCommentReportRequest
    {
              [JsonProperty("comment_id")]
        [JsonPropertyName("comment_id")]
        public string CommentId { get; set; }

              [JsonProperty("reason")]
        [JsonPropertyName("reason")]
        public IEnumerable<string> Reason { get; set; }

              [JsonProperty("external_reason")]
        [JsonPropertyName("external_reason")]
        public string ExternalReason { get; set; }


    }
}

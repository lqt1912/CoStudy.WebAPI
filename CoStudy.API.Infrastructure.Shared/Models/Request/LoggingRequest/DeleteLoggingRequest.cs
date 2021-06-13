using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class DeleteLoggingRequest
    {
              [JsonProperty("ids")]
        [JsonPropertyName("ids")]
        public IEnumerable<string> Ids { get; set; }

    }
}

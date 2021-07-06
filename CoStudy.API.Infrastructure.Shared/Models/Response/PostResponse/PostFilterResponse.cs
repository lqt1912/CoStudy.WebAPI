using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class PostFilterResponse
    {
        [JsonPropertyName("record_filtered")]
        [JsonProperty("record_filtered")]
        public int RecordFiltered { get; set; }

        [JsonProperty("record_remain")]
        [JsonPropertyName("record_remain")]
        public int RecordRemain { get; set; }

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}

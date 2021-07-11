using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ViolenceWordViewModel
    {

        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
       public class LevelViewModel
    {
              [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

              [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

              [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }


              [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

              [JsonProperty("order")]
        [JsonPropertyName("order")]
        public int? Order { get; set; }


              [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

              [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

              [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }

              [JsonProperty("is_active")]
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }
    }
}

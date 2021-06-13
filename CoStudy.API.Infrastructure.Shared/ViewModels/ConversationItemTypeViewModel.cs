using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ConversationItemTypeViewModel
    {

        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

              [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }


              [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string Code { get; set; }

              [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; }

              [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}

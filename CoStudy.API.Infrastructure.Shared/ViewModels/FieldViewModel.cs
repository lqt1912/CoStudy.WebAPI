using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class FieldViewModel :BaseViewModel
    {
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string  OId { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; } 

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime Modified_Date { get; set; } 
    }
}

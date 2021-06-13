using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class NewsFeedRequest : BaseGetAllRequest
    {
              [JsonProperty("from_date")]
        [JsonPropertyName("from_date")]
        public DateTime? FromDate { get; set; }


              [JsonProperty("to_date")]
        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }


    }
}

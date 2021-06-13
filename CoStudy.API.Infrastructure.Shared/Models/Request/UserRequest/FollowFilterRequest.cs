using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
        public class FollowFilterRequest : BaseGetAllRequest
    {
              [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

              [JsonProperty("keyword")]
        [JsonPropertyName("keyword")]
        public string KeyWord { get; set; }

              [JsonPropertyName("date_from")]
        [JsonProperty("date_from")]
        public DateTime? FromDate { get; set; }

              [JsonPropertyName("to_date")]
        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }


              [JsonPropertyName("order_type")]
        [JsonProperty("order_type")]
        public SortType OrderType { get; set; }
    }
}

using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class FollowFilterRequest
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("date_from")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("order_type")]
        public OrderType OrderType { get; set; }
    }
}

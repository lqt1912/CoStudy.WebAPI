using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class FollowFilterRequest :BaseGetAllRequest
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("date_from")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }

      

        [JsonPropertyName("order_type")]
        public OrderType OrderType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public enum PostOrder
    {
        CreatedDate,
        Upvote,
        Comment
    }

    public enum OrderType
    {
        Ascending,
        Descending
    }

    public class FilterRequest
    {
        [JsonPropertyName("date_from")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonPropertyName("field")]
        public string  Field { get; set; }

        [JsonPropertyName("order_by")]
        public PostOrder OrderBy { get; set; }

        [JsonPropertyName("order_type")]
        public OrderType OrderType { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

    }
}

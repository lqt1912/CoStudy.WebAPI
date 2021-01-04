using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public enum UserFilterType
    {
        PostCount,
        Follower
    }

    public enum OrderTypeUser
    {
        Ascending,
        Descending
    }
    public class FilterUserRequest
    {
        [JsonPropertyName("keyword")]
        public string KeyWord { get; set; }

        [JsonPropertyName("field")]
        public string Fields { get; set; }

        [JsonPropertyName("filter_type")]
        public UserFilterType? FilterType { get; set; }

        [JsonPropertyName("order_type")]
        public OrderTypeUser? OrderType { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

    }
}

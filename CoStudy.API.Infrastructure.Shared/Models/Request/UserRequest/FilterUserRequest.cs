using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
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

    public class FilterUserRequest : BaseGetAllRequest
    {
        [JsonPropertyName("keyword")]
        [JsonProperty("keyword")]
        public string KeyWord { get; set; }


        [JsonPropertyName("filter_type")]
        [JsonProperty("filter_type")]
        public UserFilterType? FilterType { get; set; }

        [JsonPropertyName("order_type")]
        [JsonProperty("order_type")]
        public OrderTypeUser? OrderType { get; set; }

        [JsonProperty("field_filter")]
        [JsonPropertyName("field_filter")]
        public IEnumerable<string> FieldFilter { get; set; }

    }
}

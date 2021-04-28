using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    /// <summary>
    /// Enum UserFilterType
    /// </summary>
    public enum UserFilterType
    {
        /// <summary>
        /// The post count
        /// </summary>
        PostCount,

        /// <summary>
        /// The follower
        /// </summary>
        Follower
    }

    /// <summary>
    /// Enum OderTypeUser
    /// </summary>
    public enum OrderTypeUser
    {
        /// <summary>
        /// The ascending
        /// </summary>
        Ascending,
        /// <summary>
        /// The descending
        /// </summary>
        Descending
    }

    /// <summary>
    /// Class FilterUserRequest
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class FilterUserRequest:BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets the key word.
        /// </summary>
        /// <value>
        /// The key word.
        /// </value>
        [JsonPropertyName("keyword")]
        [JsonProperty("keyword")]
        public string KeyWord { get; set; }

    
        /// <summary>
        /// Gets or sets the type of the filter.
        /// </summary>
        /// <value>
        /// The type of the filter.
        /// </value>
        [JsonPropertyName("filter_type")]
        [JsonProperty("filter_type")]
        public UserFilterType? FilterType { get; set; }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        [JsonPropertyName("order_type")]
        [JsonProperty("order_type")]
        public OrderTypeUser? OrderType { get; set; }

        /// <summary>
        /// Gets or sets the level filter.
        /// </summary>
        /// <value>
        /// The level filter.
        /// </value>
        [JsonProperty("field_filter")]
        [JsonPropertyName("field_filter")]
        public IEnumerable<string> FieldFilter { get; set; }

    }
}

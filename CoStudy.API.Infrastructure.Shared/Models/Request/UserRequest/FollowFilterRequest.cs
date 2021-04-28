using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    /// <summary>
    /// Class FollowFilterRequest
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class FollowFilterRequest : BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonPropertyName("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the key word.
        /// </summary>
        /// <value>
        /// The key word.
        /// </value>
        [JsonProperty("keyword")]
        [JsonPropertyName("keyword")]
        public string KeyWord { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        [JsonPropertyName("date_from")]
        [JsonProperty("date_from")]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        [JsonPropertyName("to_date")]
        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }


        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        [JsonPropertyName("order_type")]
        [JsonProperty("order_type")]
        public SortType OrderType { get; set; }
    }
}

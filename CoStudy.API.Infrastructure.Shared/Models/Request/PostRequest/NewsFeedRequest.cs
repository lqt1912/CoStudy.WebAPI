using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class NewsFeedRequest
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class NewsFeedRequest : BaseGetAllRequest
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        [JsonProperty("from_date")]
        [JsonPropertyName("from_date")]
        public DateTime? FromDate { get; set; }


        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        [JsonProperty("to_date")]
        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }


    }
}

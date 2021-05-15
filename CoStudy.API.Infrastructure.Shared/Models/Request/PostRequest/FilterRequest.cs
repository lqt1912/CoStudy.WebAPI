using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Enum PostOrder
    /// </summary>
    public enum SortObject
    {
        /// <summary>
        /// The created date
        /// </summary>
        CreatedDate,
        /// <summary>
        /// The upvote
        /// </summary>
        Upvote,
        /// <summary>
        /// The comment
        /// </summary>
        Comment
    }

    /// <summary>
    /// Enum OrderType
    /// </summary>
    public enum SortType
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
    /// 
    /// </summary>
    /// <seealso cref="CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest.BaseGetAllRequest" />
    public class FilterRequest : BaseGetAllRequest
    {

        #region Filter

        /// <summary>
        /// Gets or sets the content filter.
        /// </summary>
        /// <value>
        /// The content filter.
        /// </value>
        [JsonProperty("content_filter")]
        [JsonPropertyName("content_filter")]
        public string ContentFilter { get; set; }

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
        #endregion

        #region  Sort
        /// <summary>
        /// Gets or sets the sort object.
        /// </summary>
        /// <value>
        /// The sort object.
        /// </value>
        [JsonProperty("sort_object")]
        [JsonPropertyName("sort_object")]
        public SortObject? SortObject { get; set; }


        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>
        /// The type of the sort.
        /// </value>
        [JsonProperty("sort_type")]
        [JsonPropertyName("sort_type")]
        public SortType? SortType { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the level filter.
        /// </summary>
        /// <value>
        /// The level filter.
        /// </value>
        [JsonProperty("level_filter")]
        [JsonPropertyName("level_filter")]
        public LevelFilter LevelFilter { get; set; }

    }

    /// <summary>
    /// Class LevelFilter
    /// </summary>
    public class LevelFilter
    {
        /// <summary>
        /// Gets or sets the filter items.
        /// </summary>
        /// <value>
        /// The filter items.
        /// </value>
        [JsonProperty("filter_items")]
        [JsonPropertyName("filter_items")]
        public IEnumerable<LevelFilterItem> FilterItems { get; set; }
    }

    /// <summary>
    /// Class LevelFilterItem
    /// </summary>
    public class LevelFilterItem
    {
        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        [JsonProperty("level_id")]
        [JsonPropertyName("level_id")]
        public string LevelId { get; set; }
    }
}

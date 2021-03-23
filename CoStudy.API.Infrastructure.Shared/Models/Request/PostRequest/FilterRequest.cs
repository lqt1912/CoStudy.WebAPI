using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Shared.Models.Request.BaseRequest;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    /// <summary>
    /// Enum PostOrder
    /// </summary>
    public enum PostOrder
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
    public enum OrderType
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
        /// <summary>
        /// Gets or sets the content filter.
        /// </summary>
        /// <value>
        /// The content filter.
        /// </value>
        [JsonProperty("content_filter")]
        [JsonPropertyName("content_filter")]
        public StringFilter ContentFilter { get; set; }

        /// <summary>
        /// Gets or sets the created date filter.
        /// </summary>
        /// <value>
        /// The created date filter.
        /// </value>
        [JsonProperty("created_date_filter")]
        [JsonPropertyName("created_date_filter")]
        public DateTimeFilter CreatedDateFilter { get; set; }

        /// <summary>
        /// Gets or sets the upvote count filter.
        /// </summary>
        /// <value>
        /// The upvote count filter.
        /// </value>
        [JsonProperty("upvote_count_filter")]
        [JsonPropertyName("upvote_count_filter")]
        public NumberRangeFilter UpvoteCountFilter { get; set; }

        /// <summary>
        /// Gets or sets the comment count filter.
        /// </summary>
        /// <value>
        /// The comment count filter.
        /// </value>
        [JsonProperty("comment_count_filter")]
        [JsonPropertyName("comment_count_filter")]
        public NumberRangeFilter CommentCountFilter { get; set; }


        [JsonProperty("level_filter")]
        [JsonPropertyName("level_filter")]
        public ObjectLevel LevelFilter { get; set; }
    }

    /// <summary>
    /// Class DateTimeFilter
    /// </summary>
    public class DateTimeFilter
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        [JsonPropertyName("from_date")]
        [JsonProperty("from_date")]
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

        /// <summary>
        /// Gets or sets the is sort descending.
        /// </summary>
        /// <value>
        /// The is sort descending.
        /// </value>
        [JsonProperty("is_sort_descending")]
        [JsonPropertyName("is_sort_descending")]
        public bool? IsSortDescending { get; set; }
    }

    /// <summary>
    /// Class StringFilter
    /// </summary>
    public class StringFilter
    {
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
        /// Gets or sets the is sort descending.
        /// </summary>
        /// <value>
        /// The is sort descending.
        /// </value>
        [JsonProperty("is_sort_descending")]
        [JsonPropertyName("is_sort_descending")]
        public bool? IsSortDescending { get; set; }
    }

    /// <summary>
    /// Class NumberRangeFilter
    /// </summary>
    public class NumberRangeFilter
    {
        /// <summary>
        /// Gets or sets the value from.
        /// </summary>
        /// <value>
        /// The value from.
        /// </value>
        [JsonProperty("value_from")]
        [JsonPropertyName("value_from")]
        public int? ValueFrom { get; set; }

        /// <summary>
        /// Gets or sets the value to.
        /// </summary>
        /// <value>
        /// The value to.
        /// </value>
        [JsonProperty("value_to")]
        [JsonPropertyName("value_to")]
        public int? ValueTo { get; set; }

        /// <summary>
        /// Gets or sets the is sort descending.
        /// </summary>
        /// <value>
        /// The is sort descending.
        /// </value>
        [JsonProperty("is_sort_descending")]
        [JsonPropertyName("is_sort_descending")]
        public bool? IsSortDescending { get; set; }
    }

}

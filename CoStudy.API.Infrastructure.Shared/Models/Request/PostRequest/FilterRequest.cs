using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Common.Constants;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public enum SortObject
    {
        CreatedDate,
        Upvote,
        Comment
    }

    public enum SortType
    {
        Ascending,
        Descending
    }

    public class FilterRequest : BaseGetAllRequest
    {

        #region Filter

        [JsonProperty("content_filter")]
        [JsonPropertyName("content_filter")]
        [BsonElement("content_filter")]
        public string ContentFilter { get; set; }

        [JsonProperty("from_date")]
        [JsonPropertyName("from_date")]
        [BsonElement("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("to_date")]
        [JsonPropertyName("to_date")]
        [BsonElement("to_date")]
        public DateTime? ToDate { get; set; }
        #endregion

        #region  Sort
        [JsonProperty("sort_object")]
        [JsonPropertyName("sort_object")]
        [BsonElement("sort_object")]
        public SortObject? SortObject { get; set; }


        [JsonProperty("sort_type")]
        [JsonPropertyName("sort_type")]
        [BsonElement("sort_type")]
        public SortType? SortType { get; set; }
        #endregion

        [JsonProperty("level_filter")]
        [JsonPropertyName("level_filter")]
        [BsonElement("level_filter")]
        public LevelFilter LevelFilter { get; set; }

        [JsonProperty("post_type")]
        [JsonPropertyName("post_type")]
        [BsonElement("post_type")]
        public PostType PostType { get; set; }
    }

    public class LevelFilter
    {
        [JsonProperty("filter_items")]
        [JsonPropertyName("filter_items")]
        [BsonElement("filter_items")]
        public IEnumerable<LevelFilterItem> FilterItems { get; set; }
    }

    public class LevelFilterItem
    {
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        [BsonElement("field_id")]
        public string FieldId { get; set; }

        //[JsonProperty("level_id")]
        //[JsonPropertyName("level_id")]
        //public string LevelId { get; set; }
    }
}

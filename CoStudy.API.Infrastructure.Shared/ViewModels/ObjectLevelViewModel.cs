using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class ObjectLevelViewModel
    {
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        [JsonProperty("object_id")]
        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }

        [JsonProperty("field_name")]
        [JsonPropertyName("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("level_id")]
        [JsonPropertyName("level_id")]
        public string LevelId { get; set; }

        [JsonProperty("level_name")]
        [JsonPropertyName("level_name")]
        public string LevelName { get; set; }

        [JsonProperty("level_description")]
        [JsonPropertyName("level_description")]
        public string LevelDescription { get; set; }

        [JsonProperty("point")]
        [JsonPropertyName("point")]
        public int? Point { get; set; }

        [JsonProperty("is_active")]
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

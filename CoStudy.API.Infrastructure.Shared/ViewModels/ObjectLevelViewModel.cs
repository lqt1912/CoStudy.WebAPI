using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class Object level view model
    /// </summary>
    public class ObjectLevelViewModel
    {
        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonProperty("oid")]
        [JsonPropertyName("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [JsonProperty("object_id")]
        [JsonPropertyName("object_id")]
        public string  ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public string  FieldId { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [JsonProperty("field_name")]
        [JsonPropertyName("field_name")]
        public string  FieldName { get; set; }

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        [JsonProperty("level_id")]
        [JsonPropertyName("level_id")]
        public string LevelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        [JsonProperty("level_name")]
        [JsonPropertyName("level_name")]
        public string  LevelName { get; set; }

        [JsonProperty("level_description")]
        [JsonPropertyName("level_description")]
        public string LevelDescription { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        [JsonProperty("point")]
        [JsonPropertyName("point")]
        public int? Point { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        [JsonProperty("is_active")]
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

    }
}

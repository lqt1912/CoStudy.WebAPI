using CoStudy.API.Infrastructure.Shared.Validator;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class AddPointRequest
    /// </summary>
    public class AddPointRequest
    {
        /// <summary>
        /// Gets or sets the object level identifier.
        /// </summary>
        /// <value>
        /// The object level identifier.
        /// </value>
        [StringRequired]
        [JsonProperty("object_level_id")]
        [JsonPropertyName("object_level_id")]
        public string ObjectLevelId { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        [Required]
        [JsonProperty("point")]
        [JsonPropertyName("point")]
        public int Point { get; set; } = 0;
    }
}

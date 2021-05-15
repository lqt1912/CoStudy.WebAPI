using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    /// <summary>
    /// Class UpdatePostLevelRequest
    /// </summary>
    public class UpdatePostLevelRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }


        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        [JsonProperty("field")]
        [JsonPropertyName("field")]
        public IEnumerable<ObjectLevel> Field { get; set; }
    }
}

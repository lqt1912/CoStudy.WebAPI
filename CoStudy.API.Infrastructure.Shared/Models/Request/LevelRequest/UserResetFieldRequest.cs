using CoStudy.API.Infrastructure.Shared.Validator;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest
{
    /// <summary>
    /// Class UserResetFieldRequest
    /// </summary>
    public class UserResetFieldRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [StringRequired]
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }


        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public IEnumerable<string> FieldId { get; set; }
    }
}

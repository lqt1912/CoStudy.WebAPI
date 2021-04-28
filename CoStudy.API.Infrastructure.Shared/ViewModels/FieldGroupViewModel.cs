using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class FieldGroupViewModel
    /// </summary>
    public class FieldGroupViewModel
    {
        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        [JsonProperty("group_name")]
        [JsonPropertyName("group_name")]
        public string  GroupName { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }
}

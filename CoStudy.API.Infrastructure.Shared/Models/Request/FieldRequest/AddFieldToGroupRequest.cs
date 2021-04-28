using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.FieldRequest
{
    /// <summary>
    /// Class AddFieldToGroupRequest
    /// </summary>
    public class AddFieldToGroupRequest
    {
        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        [JsonPropertyName("group_id")]
        [JsonProperty("group_id")]
        public string  GroupId { get; set; }


        /// <summary>
        /// Gets or sets the field ids.
        /// </summary>
        /// <value>
        /// The field ids.
        /// </value>
        [JsonProperty("field_ids")]
        [JsonPropertyName("field_ids")]
        public IEnumerable<string> FieldIds { get; set; }
    }
}

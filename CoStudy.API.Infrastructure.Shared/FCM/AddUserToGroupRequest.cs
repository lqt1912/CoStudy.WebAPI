using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Application.FCM
{
    /// <summary>
    /// Class AddUserToGroupRequest
    /// </summary>
    public class AddUserToGroupRequest
    {
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        [JsonProperty("group_name")]
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("group_type")]
        [JsonPropertyName("group_type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        /// <value>
        /// The user ids.
        /// </value>
        [JsonProperty("user_ids")]
        [JsonPropertyName("user_ids")]
        public IEnumerable<string> UserIds { get; set; }

    }
}

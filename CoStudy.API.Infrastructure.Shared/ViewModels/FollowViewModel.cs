using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class FollowViewModel
    /// </summary>
    public class FollowViewModel
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
        /// Gets or sets from name.
        /// </summary>
        /// <value>
        /// From name.
        /// </value>
        [JsonProperty("from_name")]
        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        /// <summary>
        /// Converts to name.
        /// </summary>
        /// <value>
        /// To name.
        /// </value>
        [JsonProperty("to_name")]
        [JsonPropertyName("to_name")]
        public string ToName { get; set; }


        /// <summary>
        /// Gets or sets from avatar.
        /// </summary>
        /// <value>
        /// From avatar.
        /// </value>
        [JsonProperty("from_avatar")]
        [JsonPropertyName("from_avatar")]
        public string FromAvatar { get; set; }

        /// <summary>
        /// Converts to avatar.
        /// </summary>
        /// <value>
        /// To avatar.
        /// </value>
        [JsonProperty("to_avatar")]
        [JsonPropertyName("to_avatar")]
        public string ToAvatar { get; set; }

        /// <summary>
        /// Gets or sets the follow date.
        /// </summary>
        /// <value>
        /// The follow date.
        /// </value>
        [JsonProperty("follow_date")]
        [JsonPropertyName("follow_date")]
        public DateTime? FollowDate { get; set; }

        /// <summary>
        /// Gets or sets from identifier.
        /// </summary>
        /// <value>
        /// From identifier.
        /// </value>
        [JsonProperty("from_id")]
        [JsonPropertyName("from_id")]
        public string FromId { get; set; }

        /// <summary>
        /// Converts to id.
        /// </summary>
        /// <value>
        /// To identifier.
        /// </value>
        [JsonProperty("to_id")]
        [JsonPropertyName("to_id")]
        public string ToId { get; set; }

        /// <summary>
        /// Gets or sets the is follow by current.
        /// </summary>
        /// <value>
        /// The is follow by current.
        /// </value>
        [JsonProperty("is_follow_by_current")]
        [JsonPropertyName("is_follow_by_current")]
        public bool? IsFollowByCurrent { get; set; }
    }
}

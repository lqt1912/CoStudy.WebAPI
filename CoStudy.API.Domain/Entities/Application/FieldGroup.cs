using CoStudy.API.Domain.Entities.BaseEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class FieldGroup
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class FieldGroup : Entity
    {

        public FieldGroup() : base()
        {
            FieldId = new List<string>();
        }
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        [JsonPropertyName("group_name")]
        [JsonProperty("group_name")]
        public string GroupName { get; set; }


        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public List<string> FieldId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; } = ItemStatus.Active;
    }
}

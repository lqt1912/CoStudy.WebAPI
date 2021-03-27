using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Client group
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class ClientGroup : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGroup" /> class.
        /// </summary>
        public ClientGroup() : base()
        {
            UserIds = new List<string>();

        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the group.
        /// </summary>
        /// <value>
        /// The type of the group.
        /// </value>
        [BsonElement("group_type")]
        [JsonPropertyName("group_type")]
        public string GroupType { get; set; }


        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        /// <value>
        /// The user ids.
        /// </value>
        [BsonElement("user_ids")]
        [JsonPropertyName("user_ids")]
        public List<string> UserIds { get; set; }
    }
}

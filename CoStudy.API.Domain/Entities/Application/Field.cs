using System;
using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Field
    /// </summary>
    /// <seealso cref="CoStudy.API.Domain.Entities.BaseEntity.Entity" />
    public class Field : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field() : base()
        {

        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [BsonElement("value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("status")]
        [JsonProperty("status")]
        [BsonElement("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [BsonElement("modified_date")]
        public DateTime Modified_Date { get; set; } =DateTime.Now;

    }
}

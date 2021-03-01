using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class District 
    /// </summary>
    public class District
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [JsonPropertyName("code")]
        [BsonElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonPropertyName("name")]
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the province code.
        /// </summary>
        /// <value>
        /// The province code.
        /// </value>
        [JsonPropertyName("province_code")]
        [BsonElement("province_code")]
        public string ProvinceCode { get; set; }
    }
}

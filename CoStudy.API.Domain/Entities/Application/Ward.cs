using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class ward. 
    /// </summary>
    public class Ward
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
        /// Gets or sets the district code.
        /// </summary>
        /// <value>
        /// The district code.
        /// </value>
        [JsonPropertyName("district_code")]
        [BsonElement("district_code")]
        public string DistrictCode { get; set; }

    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Class Address
    /// </summary>
    public class Address
    {

        /// <summary>
        /// Gets or sets the district.
        /// </summary>
        /// <value>
        /// The district.
        /// </value>
        [BsonElement("district")]
        [JsonPropertyName("district")]
        public string District { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [BsonElement("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        [BsonElement("detail")]
        [JsonPropertyName("detail")]
        public string Detail { get; set; }


        /// <summary>
        /// Gets or sets the longtitude.
        /// </summary>
        /// <value>
        /// The longtitude.
        /// </value>
        [BsonElement("longtitude")]
        [JsonPropertyName("longtitude")]
        public string Longtitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [BsonElement("latitude")]
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

    }
}

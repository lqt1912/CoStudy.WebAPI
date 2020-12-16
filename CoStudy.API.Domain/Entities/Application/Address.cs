using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Address
    {

        [BsonElement("district")]
        [JsonPropertyName("district")]
        public string District { get; set; }

        [BsonElement("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [BsonElement("detail")]
        [JsonPropertyName("detail")]
        public string Detail { get; set; }


        [BsonElement("longtitude")]
        [JsonPropertyName("longtitude")]
        public string Longtitude { get; set; }

        [BsonElement("latitude")]
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }

    }
}

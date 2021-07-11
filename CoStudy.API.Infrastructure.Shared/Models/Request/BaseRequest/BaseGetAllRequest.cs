using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class BaseGetAllRequest
    {
        [JsonPropertyName("skip")]
        [BsonElement("skip")]
        [JsonProperty("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        [BsonElement("count")]
        [JsonProperty("count")]
        public int? Count { get; set; }

    }
}

using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class BaseGetAllRequest
    {
        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

    }
}

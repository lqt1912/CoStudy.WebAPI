using CoStudy.API.Application.Features;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class UserNearbyViewModel
    {
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonProperty("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonProperty("avatar")]
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("location")]
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();

        [JsonProperty("distance")]
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }
}

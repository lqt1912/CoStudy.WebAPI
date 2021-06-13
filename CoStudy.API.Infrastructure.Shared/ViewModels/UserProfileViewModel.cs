using CoStudy.API.Domain.Entities.Identity.MongoAuthen;
using CoStudy.API.Infrastructure.Identity.Models.GoogleAuth;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class UserProfileViewModel
    {
        [JsonPropertyName("user")]
        [JsonProperty("user")]
        public UserViewModel? User { get; set; }

        [JsonProperty("account")]
        [JsonPropertyName("account")]
        public Account? Account { get; set; }

        [JsonProperty("external_login")]
        [JsonPropertyName("external_login")]
        public ExternalLogin? ExternalLogin { get; set; }
    }
}

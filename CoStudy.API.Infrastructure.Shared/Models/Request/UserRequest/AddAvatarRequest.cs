using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddAvatarRequest
    {
        //[JsonPropertyName("image")]
        //public IFormFile Image { get; set; }

        [JsonPropertyName("description")]
        public string Discription { get; set; }

        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }
        //[JsonPropertyName("user_id")]
        //public string UserId { get; set; }

    }
}

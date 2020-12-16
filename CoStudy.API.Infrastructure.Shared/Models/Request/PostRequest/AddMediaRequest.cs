using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class AddMediaRequest
    {
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("description")]
        public string Discription { get; set; }

        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

    }
}

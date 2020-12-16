using CoStudy.API.Domain.Entities.Application;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class AddPostResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("post")]
        public Post Post { get; set; }
    }
}

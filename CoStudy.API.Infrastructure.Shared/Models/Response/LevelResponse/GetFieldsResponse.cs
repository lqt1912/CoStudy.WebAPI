using CoStudy.API.Domain.Entities.Application;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models
{
    public class GetFieldsResponse
    {
        public Field Field { get; set; }
        public Level Level { get; set; }
    }

    public class UserGetFieldResponse : GetFieldsResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("point")]
        public int? Point { get; set; }
    }


    public class PostGetFieldResponse : GetFieldsResponse
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

    }
}

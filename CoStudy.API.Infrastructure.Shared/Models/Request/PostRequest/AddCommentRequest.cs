using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class AddCommentRequest
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("image_hash")]
        public string Image { get; set; }

        [JsonPropertyName("post_id")]

        public string PostId { get; set; }

 


    }
}

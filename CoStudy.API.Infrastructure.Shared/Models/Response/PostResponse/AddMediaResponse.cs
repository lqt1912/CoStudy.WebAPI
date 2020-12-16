using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class AddMediaResponse
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }


        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }

        [JsonPropertyName("discription")]
        public string Discription { get; set; }
    }
}

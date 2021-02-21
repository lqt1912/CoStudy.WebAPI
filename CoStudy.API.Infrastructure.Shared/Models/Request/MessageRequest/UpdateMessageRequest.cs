using CoStudy.API.Domain.Entities.Application;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.MessageRequest
{
    public class UpdateMessageRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }
    }
}

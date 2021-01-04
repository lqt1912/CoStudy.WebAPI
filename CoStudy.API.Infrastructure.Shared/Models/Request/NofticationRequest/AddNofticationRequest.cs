using CoStudy.API.Domain.Entities.Application;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.NofticationRequest
{
    public class AddNofticationRequest
    {
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

        public string Content { get; set; }
    }
}

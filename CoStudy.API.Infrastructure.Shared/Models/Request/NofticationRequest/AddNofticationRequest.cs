using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class AddNofticationRequest
    {
              [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

              [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }

              public string Content { get; set; }

              [JsonPropertyName("object_id")]
        [JsonProperty("object_id")]
        public string ObjectId { get; set; }
    }
}

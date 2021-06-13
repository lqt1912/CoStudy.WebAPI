using CoStudy.API.Infrastructure.Shared.Validator;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class AddPointRequest
    {
              [StringRequired]
        [JsonProperty("object_level_id")]
        [JsonPropertyName("object_level_id")]
        public string ObjectLevelId { get; set; }

              [Required]
        [JsonProperty("point")]
        [JsonPropertyName("point")]
        public int Point { get; set; } = 0;
    }
}

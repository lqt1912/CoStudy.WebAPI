using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class UpdatePostLevelRequest
    {
              [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }


              [JsonProperty("field")]
        [JsonPropertyName("field")]
        public IEnumerable<ObjectLevel> Field { get; set; }
    }
}

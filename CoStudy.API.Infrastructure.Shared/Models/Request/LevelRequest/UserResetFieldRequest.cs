using CoStudy.API.Infrastructure.Shared.Validator;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
       public class UserResetFieldRequest
    {
              [StringRequired]
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }


              [JsonProperty("field_id")]
        [JsonPropertyName("field_id")]
        public IEnumerable<string> FieldId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.LevelRequest
{
    public class UserAddFieldRequest
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("level_id")]
        public string LevelId { get; set; }

        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddFieldRequest
    {
        [JsonPropertyName("field_value")]
        public List<string> UserField { get; set; }
    }
}

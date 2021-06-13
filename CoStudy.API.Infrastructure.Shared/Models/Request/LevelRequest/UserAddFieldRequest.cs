using CoStudy.API.Infrastructure.Shared.Validator;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class UserAddFieldRequest
    {
        [JsonPropertyName("user_id")]
        [StringRequired]
        public string UserId { get; set; }

        [ListRequired(MaxCount = 10, MinCount = 1, UpperLimitMessage = "Vượt quá giới hạn", LowerLimitMessage = "Ít hơn giới hạn")]
        [JsonPropertyName("field_id")]
        public IEnumerable<string> FieldId { get; set; }
    }
}

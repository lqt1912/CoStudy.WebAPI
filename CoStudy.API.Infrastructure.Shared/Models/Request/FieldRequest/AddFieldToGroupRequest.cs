using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request
{
    public class AddFieldToGroupRequest
    {
        [JsonPropertyName("group_id")]
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        [JsonPropertyName("group_name")]
        [JsonProperty("group_name")]
        public string  GroupName { get; set; }

        [JsonProperty("field_ids")]
        [JsonPropertyName("field_ids")]
        public IEnumerable<string> FieldIds { get; set; }
    }
}

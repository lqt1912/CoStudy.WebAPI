using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Application.FCM
{
       public class AddUserToGroupRequest
    {
              [JsonProperty("group_name")]
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }


              [JsonProperty("group_type")]
        [JsonPropertyName("group_type")]
        public string Type { get; set; }

              [JsonProperty("user_ids")]
        [JsonPropertyName("user_ids")]
        public IEnumerable<string> UserIds { get; set; }

    }
}

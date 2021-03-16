using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class FollowViewModel
    {

        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonProperty("from_name")]
        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        [JsonProperty("to_name")]
        [JsonPropertyName("to_name")]
        public string  ToName { get; set; }


        [JsonProperty("from_avatar")]
        [JsonPropertyName("from_avatar")]
        public string  FromAvatar { get; set; }

        [JsonProperty("to_avatar")]
        [JsonPropertyName("to_avatar")]
        public string  ToAvatar { get; set; }

        [JsonProperty("follow_date")]
        [JsonPropertyName("follow_date")]
        public DateTime? FollowDate { get; set; }

        [JsonProperty("from_id")]
        [JsonPropertyName("from_id")]
        public string FromId { get; set; }

        [JsonProperty("to_id")]
        [JsonPropertyName("to_id")]
        public string  ToId { get; set; }

    }
}

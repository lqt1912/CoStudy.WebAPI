using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class SavePostResponse
    {
        [JsonProperty("is_save")]
        [JsonPropertyName("is_save")]
        public bool IsSave { get; set; }

        [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }
    }
}

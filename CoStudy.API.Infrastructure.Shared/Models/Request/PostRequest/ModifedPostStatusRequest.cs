using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class ModifedPostStatusRequest
    {
        [JsonProperty("post_id")]
        [JsonPropertyName("post_id")]
        public string PostId { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status  { get; set; }
    }
}

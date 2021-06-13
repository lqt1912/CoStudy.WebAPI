using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class ModifiedRequest
    {
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }
    }
}

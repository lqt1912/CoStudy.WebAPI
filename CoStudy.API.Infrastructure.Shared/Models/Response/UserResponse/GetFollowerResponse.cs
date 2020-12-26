using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse
{
    public class GetFollowerResponse
    {
        [JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }
    }
}

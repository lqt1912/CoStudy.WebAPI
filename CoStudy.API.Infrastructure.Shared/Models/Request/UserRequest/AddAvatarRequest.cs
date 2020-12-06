using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.UserRequest
{
    public class AddAvatarRequest
    {
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        [JsonPropertyName("description")]
        public string Discription { get; set; }

        //[JsonPropertyName("user_id")]
        //public string UserId { get; set; }

    }
}

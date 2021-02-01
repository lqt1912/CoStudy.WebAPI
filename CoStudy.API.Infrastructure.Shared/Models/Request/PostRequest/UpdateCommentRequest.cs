using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class UpdateCommentRequest
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("image_hash")]
        public string Image { get; set; }

        [JsonPropertyName("id")]

        public string Id { get; set; }
    }
}

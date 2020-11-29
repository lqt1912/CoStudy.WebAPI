using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse
{
    public class AddAvatarResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("id")]
        public ObjectId Id { get; set; }

        [JsonPropertyName("discription")]
        public string Discription { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("base64_string")]
        public string Base64String { get; set; }

        [JsonPropertyName("original_width")]
        public int OriginalWidth { get; set; }

        [JsonPropertyName("original_height")]
        public int OriginalHeight { get; set; }


        [JsonPropertyName("compress_ratio")]
        public double CompressRatio { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}

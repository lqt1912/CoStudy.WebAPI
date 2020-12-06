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

    

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}

using CoStudy.API.Domain.Entities.BaseEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Domain.Entities.Application
{
    public class User : Entity
    {
        [BsonElement("first_name")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [BsonElement("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [BsonElement("date_of_birth")]
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [BsonElement("phone_number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [BsonElement("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }


        [BsonElement("avatar")]
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }

        [BsonElement("avatar_hash")]
        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }

        [BsonElement("call_id")]
        [JsonPropertyName("call_id")]
        public string CallId { get; set; }

        [BsonElement("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public string LatestRefreshToken { get; set; }

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [BsonElement("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfomation> AdditionalInfos { get; set; }

        [BsonElement("post_saved")]
        [JsonPropertyName("post_saved")]
        public List<string> PostSaved { get; set; }

        [BsonElement("jwt_tokens")]
        [JsonPropertyName("jwt_tokens")]
        public List<string> JwtTokens { get; set; }

        [BsonElement("notification_off")]
        [JsonPropertyName("notification_off")]
        public List<string> TurnOfNotification { get; set; }
        public User() : base()
        {
            Avatar = new Image();
            AdditionalInfos = new List<AdditionalInfomation>();
            PostSaved = new List<string>();
            JwtTokens = new List<string>();
            TurnOfNotification = new List<string>();
        }
    }
}

using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class UserViewModel
    {
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonProperty("date_of_birth")]
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }


        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonProperty("avatar")]
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }

        [JsonProperty("avatar_hash")]
        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("post_count")]
        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        [JsonProperty("followers")]
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        [JsonProperty("followings")]
        [JsonPropertyName("followings")]
        public int Following { get; set; }

        [JsonProperty("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<IDictionary<string, string>> AdditionalInfos { get; set; }

        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<string> Fields { get; set; }

        [JsonProperty("post_upvote")]
        [JsonPropertyName("post_upvote")]
        public List<string> PostUpvote { get; set; }

        [JsonProperty("post_downvote")]
        [JsonPropertyName("post_downvote")]
        public List<string> PostDownvote { get; set; }

        [JsonProperty("post_saved")]
        public List<string> PostSaved { get; set; }
    }
}

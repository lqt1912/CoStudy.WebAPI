using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class UserViewModel.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the o identifier.
        /// </summary>
        /// <value>
        /// The o identifier.
        /// </value>
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonPropertyName("first_name")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [JsonProperty("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [JsonProperty("date_of_birth")]
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [JsonPropertyName("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [JsonProperty("phone_number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }


        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the full address.
        /// </summary>
        /// <value>
        /// The full address.
        /// </value>
        [JsonProperty("full_address")]
        [JsonPropertyName("full_address")]
        public string FullAddress { get; set; }

        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>
        /// The avatar.
        /// </value>
        [JsonProperty("avatar")]
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }

        /// <summary>
        /// Gets or sets the avatar hash.
        /// </summary>
        /// <value>
        /// The avatar hash.
        /// </value>
        [JsonProperty("avatar_hash")]
        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the post count.
        /// </summary>
        /// <value>
        /// The post count.
        /// </value>
        [JsonProperty("post_count")]
        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the followers.
        /// </summary>
        /// <value>
        /// The followers.
        /// </value>
        [JsonProperty("followers")]
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Gets or sets the following.
        /// </summary>
        /// <value>
        /// The following.
        /// </value>
        [JsonProperty("followings")]
        [JsonPropertyName("followings")]
        public int Following { get; set; }

        /// <summary>
        /// Gets or sets the additional infos.
        /// </summary>
        /// <value>
        /// The additional infos.
        /// </value>
        [JsonProperty("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfomation> AdditionalInfos { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<object> Fields { get; set; } = new List<object>();

        /// <summary>
        /// Gets or sets the post upvote.
        /// </summary>
        /// <value>
        /// The post upvote.
        /// </value>
        [JsonProperty("post_upvote")]
        [JsonPropertyName("post_upvote")]
        public List<string> PostUpvote { get; set; }

        /// <summary>
        /// Gets or sets the post downvote.
        /// </summary>
        /// <value>
        /// The post downvote.
        /// </value>
        [JsonProperty("post_downvote")]
        [JsonPropertyName("post_downvote")]
        public List<string> PostDownvote { get; set; }

        /// <summary>
        /// Gets or sets the post saved.
        /// </summary>
        /// <value>
        /// The post saved.
        /// </value>
        [JsonProperty("post_saved")]
        [JsonPropertyName("post_saved")]
        public List<string> PostSaved { get; set; }

        /// <summary>
        /// Gets or sets the call identifier.
        /// </summary>
        /// <value>
        /// The call identifier.
        /// </value>
        [JsonProperty("call_id")]
        [JsonPropertyName("call_id")]
        public string CallId { get; set; }

    }
}

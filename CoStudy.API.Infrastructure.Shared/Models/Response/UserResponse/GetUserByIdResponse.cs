﻿using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.UserResponse
{
    public class GetUserByIdResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }


        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }


        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }


        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }


        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("moified_date")]
        public DateTime ModifiedDate { get; set; }


            [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; }

        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        [JsonPropertyName("noftications")]
        public List<Noftication> Noftications { get; set; }


            [JsonPropertyName("followers")]
        public int Followers { get; set; }


            [JsonPropertyName("following")]
        public List<string> Following { get; set; }

            [JsonPropertyName("additional_infos")]
        public List<IDictionary<string, string>> AdditionalInfos { get; set; }

            [JsonPropertyName("fortes")]
        public List<Field> Fortes { get; set; }
        [JsonPropertyName("avatar_hash")]
        public string ImageHash { get; set; }

        [JsonPropertyName("post_upvote")]
        public List<string> PostUpvote { get; set; }

        [JsonPropertyName("post_downvote")]
        public List<string> PostDownvote { get; set; }

    }
}

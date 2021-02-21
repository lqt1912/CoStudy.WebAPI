﻿using CoStudy.API.Domain.Entities.BaseEntity;
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

        [BsonElement("status")]
        [JsonPropertyName("status")]
        public ItemStatus Status { get; set; }

        [BsonElement("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }




        [BsonElement("post_count")]
        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        /// <summary>
        /// List id of follower
        /// </summary>
        [BsonElement("followers")]
        [JsonPropertyName("followers")]
        public int Followers { get; set; }


        /// <summary>
        /// List id of followings
        /// </summary>
        [BsonElement("followings")]
        [JsonPropertyName("followings")]
        public int Following { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// 
        [BsonElement("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<IDictionary<string, string>> AdditionalInfos { get; set; }


        [BsonElement("fields")]
        [JsonPropertyName("fields")]
        public List<string> Fields { get; set; }



        [BsonElement("post_upvote")]
        [JsonPropertyName("post_upvote")]
        public List<string> PostUpvote { get; set; }

        [BsonElement("post_downvote")]
        [JsonPropertyName("post_downvote")]
        public List<string> PostDownvote { get; set; }

        [BsonElement("post_saved")]
        [JsonPropertyName("post_saved")]
        public List<string> PostSaved { get; set; }
        public User() : base()
        {
            Avatar = new Image();

            AdditionalInfos = new List<IDictionary<string, string>>();
            PostDownvote = new List<string>();
            PostUpvote = new List<string>();
            PostSaved = new List<string>();
            Fields = new List<string>();
        }
    }
}

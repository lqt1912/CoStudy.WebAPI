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
        [JsonPropertyName("avaavatar_hashtar")]
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


        /// <summary>
        /// List id of post written by this id
        /// </summary>
        [BsonElement("posts")]
        [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; }


        [BsonElement("noftications")]
        [JsonPropertyName("noftications")]
        public List<Noftication> Noftications { get; set; }

        [BsonElement("post_count")]
        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        /// <summary>
        /// List id of follower
        /// </summary>
        [BsonElement("followers")]
        [JsonPropertyName("followers")]
        public List<string> Followers { get; set; }


        /// <summary>
        /// List id of followings
        /// </summary>
        [BsonElement("followings")]
        [JsonPropertyName("followings")]
        public List<string> Following { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        /// 
        [BsonElement("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfo> AdditionalInfos { get; set; }

        /// <summary>
        /// Sở trường
        /// </summary>
        /// 
        [BsonElement("fortes")]
        [JsonPropertyName("fortes")]
        public List<Field> Fortes { get; set; }

        [BsonElement("client_connections_id")]
        [JsonPropertyName("client_connections_id")]
        public string ClientConnectionsId { get; set; }

        [BsonElement("post_upvote")]
        public List<string> PostUpvote { get; set; }

        [BsonElement("post_downvote")]
        public List<string> PostDownvote { get; set; }
        public User() : base()
        {
            Avatar = new Image();
            Posts = new List<Post>();
            Noftications = new List<Noftication>();
            Followers = new List<string>();
            Following = new List<string>();

            AdditionalInfos = new List<AdditionalInfo>();
            Fortes = new List<Field>();
            PostDownvote = new List<string>();
            PostUpvote = new List<string>();
        }
    }
}
